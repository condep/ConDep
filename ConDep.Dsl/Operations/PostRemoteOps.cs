using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Operations
{
    internal class PostRemoteOps
    {
        public void Configure(PostOpsSequence sequence)
        {
            var script = string.Format(@"add-type -AssemblyName System.ServiceProcess
$service = get-service condepnode

if($service) {{ 
    $service.Stop()
    $service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
    $wmiService = Get-WmiObject -Class Win32_Service -Filter ""Name='condepnode'"" 
    $wmiService.Delete()
}} 

Remove-Item -force -recurse {0}{1}",
                    @"$env:windir\temp\ConDep\", ConDepGlobals.ExecId);
            var op = new RemotePowerShellHostOperation(script);
            sequence.Add(op);
        }
    }
}