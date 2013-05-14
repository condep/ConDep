using System.IO;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class RemotePowerShellHostOperation : RemoteOperation
    {
        private readonly string _cmd;
        private Pipeline _pipeline;
        private PowerShellExecutor _executor;

        public RemotePowerShellHostOperation(string cmd)
        {
            _cmd = cmd;
            _executor = new PowerShellExecutor();
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _executor.Execute(server, _cmd);
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

    public abstract class RemoteOperation : IOperateRemote
    {
        public abstract void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings);
        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);

    }
}