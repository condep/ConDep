using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteOperationsBuilder : IOfferRemoteOperations
    {
        public RemoteOperationsBuilder(RemoteSequence remoteSequence)
        {

            Configure.DeploymentOperations = new RemoteDeploymentBuilder(remoteSequence);
            Configure.ExecutionOperations = new RemoteExecutionBuilder(remoteSequence);

        }

        public IOfferRemoteDeployment Deploy { get { return (RemoteDeploymentBuilder) Configure.DeploymentOperations; } }
        public IOfferRemoteExecution ExecuteRemote { get { return (RemoteExecutionBuilder) Configure.ExecutionOperations; } }
    }
}