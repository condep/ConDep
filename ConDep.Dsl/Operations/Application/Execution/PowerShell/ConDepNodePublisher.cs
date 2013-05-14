using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class ConDepNodePublisher : IDisposable
    {
        private readonly byte[] _nodeExe;
        private readonly string _nodeDestPath;
        private readonly string _nodeListenUrl;
        private const string SHELL_URI = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        private Runspace _runspace;

        public ConDepNodePublisher(byte[] nodeExe, string nodeDestPath, string nodeListenUrl)
        {
            _nodeExe = nodeExe;
            _nodeDestPath = nodeDestPath;
            _nodeListenUrl = nodeListenUrl;
        }

        public void Execute(ServerConfig server)
        {
            var host = new ConDepPSHost();

            var remoteCredential = new PSCredential(server.DeploymentUser.UserName, server.DeploymentUser.PasswordAsSecString);
            var connectionInfo = new WSManConnectionInfo(false, server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential, 5*60*1000);
            //{AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            _runspace = RunspaceFactory.CreateRunspace(host, connectionInfo);
            _runspace.Open();
            var openFileStream = System.Management.Automation.PowerShell.Create()
                .AddScript(string.Format(
                    @"Param([string]$remFile, $data, $bytes)
    $remFile = $ExecutionContext.InvokeCommand.ExpandString($remFile)
    $dir = Split-Path $remFile

    $dirInfo = [IO.Directory]::CreateDirectory($dir)
    [IO.FileStream]$filestream = [IO.File]::OpenWrite( $remFile )
    $filestream.Write( $data, 0, $bytes )
    $filestream.Close()
    New-Service -Name ConDepNode -BinaryPathName ""$remFile {0}"" -StartupType Manual
    Start-Service ConDepNode
", _nodeListenUrl))
                .AddParameter("remFile", _nodeDestPath).AddParameter("data", _nodeExe).AddParameter("bytes", _nodeExe.Length);
            openFileStream.Runspace = _runspace;

            var fileStreamResult = openFileStream.Invoke();
            foreach (var psObject in fileStreamResult)
            {
                Logger.Verbose(psObject.ToString());
            }
        }

        public void Dispose()
        {
            Logger.Info("Disposing!!!");            
        }
    }
}