using ConDep.Dsl.Experimental.Application.Dsl.Remote;

namespace ConDep.Dsl.Experimental.Core
{
    public interface IManageRemoteSequence : ISequenceElement
    {
        void Add(IOperateRemote remoteOperation);
        //IManageLocalSequence NewLocalSequence();
        //void Add(RemoteCompositeOperation remoteOperation);
    }
}