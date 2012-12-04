using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IRemoteSequenceElement
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status);
    }
}