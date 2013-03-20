using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public interface IOperateRemote : IExecuteOnServer
    {
        bool IsValid(Notification notification);
    }
}