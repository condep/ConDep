using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    public interface IManageRemoteSequence : IManageSequence<IOperateRemote>
    {
        CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation);
    }
}