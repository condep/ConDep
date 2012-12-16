using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public interface IOperateRemote
    {
        bool IsValid(Notification notification);
        IReportStatus Execute(ServerConfig server, IReportStatus status);
    }
}