using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Remote
{
    public class ConDepNodePublisher : IDisposable
    {
        private readonly byte[] _nodeExe;
        private readonly string _nodeDestPath;
        private readonly string _nodeListenUrl;
        private readonly ConDepSettings _settings;

        public ConDepNodePublisher(byte[] nodeExe, string nodeDestPath, string nodeListenUrl, ConDepSettings settings)
        {
            _nodeExe = nodeExe;
            _nodeDestPath = nodeDestPath;
            _nodeListenUrl = nodeListenUrl;
            _settings = settings;
        }

        public void Execute(ServerConfig server)
        {
            var script = string.Format(
                @"Param([string]$remFile, $data, $bytes)
    Set-ExecutionPolicy RemoteSigned -Force
    $remFile = $ExecutionContext.InvokeCommand.ExpandString($remFile)
    $dir = Split-Path $remFile

    $dirInfo = [IO.Directory]::CreateDirectory($dir)
    [IO.FileStream]$filestream = [IO.File]::OpenWrite( $remFile )
    $filestream.Write( $data, 0, $bytes )
    $filestream.Close()

    add-type -AssemblyName System.ServiceProcess
    $wmiService = Get-WmiObject -Class Win32_Service -Filter ""Name='condepnode'""

    if($wmiService) {{
        write-host 'ConDepNode allready exist. Stopping and removing now...'
        if($wmiService.State -ne ""Stopped"") {{
            write-host 'ConDepNode is not stopped. Stopping now...'
    	    $service = Get-Service condepnode -ErrorAction Stop
			$service.Stop()
			$service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
        }}
        write-host 'Deleting ConDepNode now...'
        $deleteResult = $wmiService.Delete()
        if($deleteResult.ReturnValue -ne 0) {{
            throw ""Failed to delete ConDepNode service. Return code was $($deleteResult.ReturnValue)""
        }}
        write-host 'ConDepNode deleted'
    }}

    write-host 'Creating new ConDepNode...'
    New-Service -Name ConDepNode -BinaryPathName ""$remFile {0}"" -StartupType Manual
    write-host 'ConDepNode created'
    write-host 'Starting ConDepNode...'
    $service = get-service condepnode
    $service.Start()
    write-host 'Waiting for node to start...'
	$service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Running)
    write-host 'ConDepNode started'
", _nodeListenUrl);

            var parameters = new List<CommandParameter>
                                 {
                                     new CommandParameter("remFile", _nodeDestPath),
                                     new CommandParameter("data", _nodeExe),
                                     new CommandParameter("bytes", _nodeExe.Length)
                                 };

            var executor = new PowerShellExecutor(server) {LoadConDepModule = false};
            var result = executor.Execute(script, parameters: parameters, logOutput: false);
            foreach (var psObject in result)
            {
                Logger.Verbose(psObject.ToString());
            }
        }

        public bool ValidateNode(string nodeListenUrl, string userName, string password)
        {
            var api = new Node.Api(nodeListenUrl, userName, password, _settings.Options.ApiTimout);
            if (!api.Validate())
            {
                Thread.Sleep(1000);
                return api.Validate();
            }
            return true;
        }

        public void Dispose()
        {
            Logger.Info("Disposing!!!");            
        }
    }
}