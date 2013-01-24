using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteOperationsBuilder : IOfferRemoteOperations
    {
        public RemoteOperationsBuilder(RemoteSequence remoteSequence, IHandleWebDeploy webDeploy)
        {

            Configure.DeploymentOperations = new RemoteDeploymentBuilder(remoteSequence, webDeploy);
            Configure.ExecutionOperations = new RemoteExecutionBuilder(remoteSequence, webDeploy);

        }

        public IOfferRemoteDeployment Deploy { get { return (RemoteDeploymentBuilder) Configure.DeploymentOperations; } }
        public IOfferRemoteExecution ExecuteRemote { get { return (RemoteExecutionBuilder) Configure.ExecutionOperations; } }
    }
}