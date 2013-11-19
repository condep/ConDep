using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Builders
{
    public class RemoteOperationsBuilder : IOfferRemoteOperations
    {
        private readonly RemoteSequence _remoteSequence;

        public RemoteOperationsBuilder(RemoteSequence remoteSequence)
        {
            _remoteSequence = remoteSequence;

            Configure.DeploymentOperations = new RemoteDeploymentBuilder(remoteSequence);
            Configure.ExecutionOperations = new RemoteExecutionBuilder(remoteSequence);

        }

        public IOfferRemoteDeployment Deploy { get { return (RemoteDeploymentBuilder) Configure.DeploymentOperations; } }
        public IOfferRemoteExecution ExecuteRemote { get { return (RemoteExecutionBuilder) Configure.ExecutionOperations; } }

        public IOfferRemoteComposition OnlyIf(Predicate<ServerInfo> condition)
        {
            return new RemoteCompositeBuilder(_remoteSequence.NewConditionalCompositeSequence(condition));
        }
    }
}