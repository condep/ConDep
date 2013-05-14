using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Operations
{
    internal class PostRemoteOps
    {
        public void Configure(PostOpsSequence sequence)
        {
            var op = new RemotePowerShellHostOperation(string.Format("Stop-Service ConDepNode; Remove-Item -force -recurse {0}{1};", @"$env:windir\temp\ConDep\", ConDepGlobals.ExecId));
            sequence.Add(op);
        }
    }
}