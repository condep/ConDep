using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class RemotePowerShellHostOperation : RemoteOperation
    {
        private readonly string _cmd;
        private readonly FileInfo _scriptFile;
        private readonly PowerShellOptions.PowerShellOptionValues _values;
        private PowerShellExecutor _executor;

        public RemotePowerShellHostOperation(string cmd, PowerShellOptions.PowerShellOptionValues values = null)
        {
            _cmd = cmd;
            _values = values;
            _executor = new PowerShellExecutor();
        }

        public RemotePowerShellHostOperation(FileInfo scriptFile, PowerShellOptions.PowerShellOptionValues values = null)
        {
            _scriptFile = scriptFile;
            _values = values;
            _executor = new PowerShellExecutor();
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            var libImport = "";
            if (_values != null && _values.RequireRemoteLib)
            {
                var copyFileOperation = new CopyFileOperation(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll"), Path.Combine(server.TempFolderDos, "ConDep.Remote.dll"));
                copyFileOperation.Execute(server, status, settings);

                //server.Deploy.File(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll"), @"%temp%\ConDep.Remote.dll");
                libImport = string.Format(@"Add-Type -Path ""{0}\ConDep.Remote.dll"";", server.TempFolderPowerShell);
            }

            _executor.Execute(server, string.Format("set-executionpolicy remotesigned -force; {0}{1}", libImport, _cmd));
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