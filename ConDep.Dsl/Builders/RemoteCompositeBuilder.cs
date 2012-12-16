using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteCompositeBuilder : IOfferRemoteComposition
    {
        private readonly IOfferRemoteCertDeployment _certDeployment;

        public RemoteCompositeBuilder(CompositeSequence compositeSequence, IOperateWebDeploy webDeploy)
        {
            Deploy = new RemoteDeploymentBuilder(compositeSequence, webDeploy);
            ExecuteRemote = new RemoteExecutionBuilder(compositeSequence, webDeploy);
        }

        public IOfferRemoteDeployment Deploy { get; private set; }
        public IOfferRemoteExecution ExecuteRemote { get; private set; }

    }
}