using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Operations
{
    internal class PostRemoteOps
    {
        public void Configure(PostOpsSequence sequence)
        {
            //var op = new RemotePowerShellHostOperation(string.Format("add-type -AssemblyName System.ServiceProcess; $service = get-service condepnode; if($service) {{ $service.Stop() | Out-String; $service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped); $wmiService = Get-WmiObject -Class Win32_Service -Filter \"Name='condepnode'\"; $wmiService.Delete() | Out-String; }} Remove-Item -force -recurse {0}{1};", @"$env:windir\temp\ConDep\", ConDepGlobals.ExecId));
            //sequence.Add(op);
        }
    }
}