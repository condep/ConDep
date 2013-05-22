using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Remote
{
    internal class PowerShellExecutor
    {
        private const string SHELL_URI = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public IEnumerable<dynamic> Execute(ServerConfig server, string command, bool logOutput = true, bool loadConDepModule = true, IEnumerable<CommandParameter> parameters = null)
        {
            var host = new ConDepPSHost();

            var remoteCredential = new PSCredential(server.DeploymentUser.UserName, server.DeploymentUser.PasswordAsSecString);
            var connectionInfo = new WSManConnectionInfo(false, server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential);
            //{AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            using (var runspace = RunspaceFactory.CreateRunspace(host, connectionInfo))
            {
                runspace.Open();

                if(logOutput) Logger.Info("Executing PowerShell command: " + command);
                var ps = PowerShell.Create();
                ps.Runspace = runspace;

                using (var pipeline = ps.Runspace.CreatePipeline("set-executionpolicy remotesigned -force"))
                {
                    if (loadConDepModule)
                    {
                        var conDepModule = string.Format(@"Import-Module $env:windir\temp\ConDep\{0}\PSScripts\ConDep;", ConDepGlobals.ExecId);
                        pipeline.Commands.AddScript(conDepModule);
                    }

                    if(parameters != null)
                    {
                        var cmd = new Command(command, true);
                        foreach(var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                        pipeline.Commands.Add(cmd);
                    }
                    else
                    {
                        pipeline.Commands.AddScript(command);
                    }

                    var result = pipeline.Invoke();
                    foreach (var psObject in result)
                    {
                        if(logOutput) Logger.Info(psObject.ToString());
                    }
                    return result;
                }
            }
        }    
    }
}