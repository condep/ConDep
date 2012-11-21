using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core
{
    public interface IRemoteSequenceElement
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status);
    }
}