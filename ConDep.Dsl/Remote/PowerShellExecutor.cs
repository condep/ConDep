using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Remote
{
    internal class PowerShellExecutor
    {
        private readonly ServerConfig _server;
        private bool _loadConDepModule = true;
        private bool _loadConDepDotNetLibrary = false;

        private const string SHELL_URI = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public PowerShellExecutor(ServerConfig server)
        {
            _server = server;
        }

        public bool LoadConDepModule
        {
            get { return _loadConDepModule; }
            set { _loadConDepModule = value; }
        }

        public bool LoadConDepDotNetLibrary
        {
            get { return _loadConDepDotNetLibrary; }
            set { _loadConDepDotNetLibrary = value; }
        }

        public IEnumerable<dynamic> Execute(string commandOrScript, IEnumerable<CommandParameter> parameters = null, bool logOutput = true)
        {
            var host = new ConDepPSHost();

            var remoteCredential = new PSCredential(_server.DeploymentUser.UserName, GetPasswordAsSecString(_server.DeploymentUser.Password));
            var connectionInfo = new WSManConnectionInfo(false, _server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential);
            //{AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            using (var runspace = RunspaceFactory.CreateRunspace(host, connectionInfo))
            {
                runspace.Open();

                Logger.Info("Executing command/script...");
                Logger.Verbose(commandOrScript);
                var ps = PowerShell.Create();
                ps.Runspace = runspace;

                using (var pipeline = ps.Runspace.CreatePipeline("set-executionpolicy remotesigned -force"))
                {
                    if (_loadConDepModule)
                    {
                        var conDepModule = string.Format(@"Import-Module $env:windir\temp\ConDep\{0}\PSScripts\ConDep;", ConDepGlobals.ExecId);
                        pipeline.Commands.AddScript(conDepModule);
                    }

                    if(_loadConDepDotNetLibrary)
                    {
                        var netLibraryCmd = string.Format(@"Add-Type -Path ""{0}\ConDep.Remote.dll"";", _server.GetServerInfo().TempFolderPowerShell);
                        pipeline.Commands.AddScript(netLibraryCmd);
                    }

                    if(parameters != null)
                    {
                        var cmd = new Command(commandOrScript, true);
                        foreach(var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                        pipeline.Commands.Add(cmd);
                    }
                    else
                    {
                        pipeline.Commands.AddScript(commandOrScript);
                    }

                    var result = pipeline.Invoke();

                    if (pipeline.Error.Count > 0)
                    {
                        var errorCollection = new PowerShellErrors();
                        foreach (var exception in pipeline.Error.NonBlockingRead().OfType<ErrorRecord>())
                        {
                            errorCollection.Add(exception.Exception);
                        }
                        throw errorCollection;
                    }

                    foreach (var psObject in result.Where(psObject => logOutput))
                    {
                        Logger.Info(psObject.ToString());
                    }
                    
                    return result;
                }
            }

        }

        public SecureString GetPasswordAsSecString(string password)
        {
            var secureString = new SecureString();
            if (!string.IsNullOrWhiteSpace(password))
            {
                password.ToCharArray().ToList().ForEach(secureString.AppendChar);
            }
            return secureString;
        }

    }
}