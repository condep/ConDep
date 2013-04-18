using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Operations
{
    internal class PostRemoteOps
    {
        public void Configure(PostOpsSequence sequence)
        {
            var op = new RemotePowerShellHostOperation(string.Format("Remove-Item -force -recurse {0}{1}", @"$env:windir\temp\ConDep\", ConDepGlobals.ExecId));
            sequence.Add(op);
            //var compSeq = sequence.NewCompositeSequence(op);
            //op.Configure(new RemoteCompositeBuilder(compSeq, new WebDeployHandler()));
        }
    }
}