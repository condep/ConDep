namespace ConDep.Dsl.Experimental.Core
{
    public interface IManageRemoteSequence : ISequenceElement
    {
        void Add(IOperateRemote remoteOperation);
        //IManageLocalSequence NewLocalSequence();
    }
}