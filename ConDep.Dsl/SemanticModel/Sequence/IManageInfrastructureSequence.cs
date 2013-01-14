using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageInfrastructureSequence
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options);
        CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation);
        bool IsvValid(Notification notification);
    }
}