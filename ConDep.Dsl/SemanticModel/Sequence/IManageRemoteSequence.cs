using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageRemoteSequence : IManageSequence<IOperateRemote>
    {
        CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation);
    }
}