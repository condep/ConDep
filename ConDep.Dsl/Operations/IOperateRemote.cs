using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public interface IOperateRemote
    {
        bool IsValid(Notification notification);
        IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions envConfig);
    }
}