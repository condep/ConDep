namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageRemoteSequence : ISequenceElement
    {
        void Add(IOperateRemote remoteOperation);
        //IManageLocalSequence NewLocalSequence();
        //void Add(RemoteCompositeOperation remoteOperation);
    }
}