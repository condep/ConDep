using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Local;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class RemotePowerShellHostOperation : LocalOperation
    {
        private const string SHELL_URI = "https://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        private readonly ServerConfig _server;
        private readonly string _cmd;

        public RemotePowerShellHostOperation(ServerConfig server, string cmd)
        {
            _server = server;
            _cmd = cmd;
        }

        public override void Execute(IReportStatus status, ConDepSettings settings)
        {
            var remoteCredential = new PSCredential(_server.DeploymentUser.UserName, _server.DeploymentUser.PasswordAsSecString);
            var connectionInfo = new WSManConnectionInfo(true, _server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential)
                                     {AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            using (var runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                runspace.Open();

                var pipeline = runspace.CreatePipeline(_cmd);
                var result = pipeline.Invoke();
            }
        }

        public override string Name
        {
            get { return "Remote PowerShell"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}