using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    public interface IManageInfrastructureSequence : IManageRemoteSequence
    {
        IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options);
        CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation);
    }
}