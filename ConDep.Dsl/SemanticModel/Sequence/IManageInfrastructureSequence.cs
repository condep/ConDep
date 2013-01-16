using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageInfrastructureSequence
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options);
        CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation);
        CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation);
        bool IsvValid(Notification notification);
    }
}