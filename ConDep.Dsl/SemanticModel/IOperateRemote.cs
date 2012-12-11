using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    public interface IOperateRemote : IRemoteSequenceElement
    {
        bool IsValid(Notification notification);
    }
}