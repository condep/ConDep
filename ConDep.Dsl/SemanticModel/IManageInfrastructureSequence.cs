using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    public interface IManageInfrastructureSequence : IManageRemoteSequence, IExecuteOnServer
    {
        CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation);
        InfrastructureSequence NewConditionalInfrastructureSequence(Predicate<ServerInfo> condition);
    }
}