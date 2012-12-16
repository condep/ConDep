namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageRemoteSequence : IManageSequence<IOperateRemote>
    {
        CompositeSequence NewCompositeSequence(string compositeName);
    }
}