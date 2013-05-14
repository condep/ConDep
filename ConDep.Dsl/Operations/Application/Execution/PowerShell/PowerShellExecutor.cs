using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    internal class PowerShellExecutor
    {
        private const string SHELL_URI = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public IEnumerable<dynamic> Execute(ServerConfig server, string command, bool logOutput = true)
        {
            var host = new ConDepPSHost();

            var remoteCredential = new PSCredential(server.DeploymentUser.UserName, server.DeploymentUser.PasswordAsSecString);
            var connectionInfo = new WSManConnectionInfo(false, server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential);
            //{AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            using (var runspace = RunspaceFactory.CreateRunspace(host, connectionInfo))
            {
                runspace.Open();

                var conDepModule = string.Format(@"Import-Module $env:windir\temp\ConDep\{0}\PSScripts\ConDep;", ConDepGlobals.ExecId);
                var psCmd = string.Format(@"set-executionpolicy remotesigned -force; {0} {1};", conDepModule, command);

                if(logOutput) Logger.Info("Executing PowerShell command: " + command);
                var ps = System.Management.Automation.PowerShell.Create();
                ps.Runspace = runspace;

                using(var pipeline = ps.Runspace.CreatePipeline(psCmd))
                {
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