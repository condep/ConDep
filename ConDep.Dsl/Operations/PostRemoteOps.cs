using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Operations
{
    internal class PostRemoteOps : IOperateRemote
    {
        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.Info("Removing ConDepNode from server...");
            var script = string.Format(@"add-type -AssemblyName System.ServiceProcess
$service = get-service condepnode

if($service) {{ 
    $service.Stop()
    $service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
    $wmiService = Get-WmiObject -Class Win32_Service -Filter ""Name='condepnode'"" 
    $wmiService.Delete() | Out-Null
}} 

Remove-Item -force -recurse {0}{1}",
                    @"$env:windir\temp\ConDep\", ConDepGlobals.ExecId);
            var executor = new PowerShellExecutor(server) {LoadConDepModule = false};
            executor.Execute(script);
        }

        public string Name { get { return "Post Remote Operation"; } }
        public bool IsValid(Notification notification)
        {
            return true;
        }
    }
}