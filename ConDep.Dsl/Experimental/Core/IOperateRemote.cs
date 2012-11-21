namespace ConDep.Dsl.Experimental.Core
{
    public interface IOperateRemote : IRemoteSequenceElement
    {
        bool IsValid(Notification notification);
    }
}