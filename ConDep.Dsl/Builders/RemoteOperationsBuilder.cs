using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteOperationsBuilder : IOfferRemoteOperations
    {
        public RemoteOperationsBuilder(RemoteSequence remoteSequence, IOperateWebDeploy webDeploy)
        {
 
            Deploy = new RemoteDeploymentBuilder(remoteSequence, webDeploy);
            ExecuteRemote = new RemoteExecutionBuilder(remoteSequence, webDeploy);
        }

        public IOfferRemoteDeployment Deploy { get; private set; }
        public IOfferRemoteExecution ExecuteRemote { get; private set; }
    }
}