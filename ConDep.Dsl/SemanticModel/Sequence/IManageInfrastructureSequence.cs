using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageInfrastructureSequence
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status);
        CompositeSequence NewCompositeSequence(string compositeName);
        bool IsvValid(Notification notification);
    }
}