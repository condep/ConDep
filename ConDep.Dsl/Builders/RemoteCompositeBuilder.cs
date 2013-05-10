using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteCompositeBuilder : IOfferRemoteComposition
    {
        private readonly IOfferRemoteCertDeployment _certDeployment;

        public RemoteCompositeBuilder(CompositeSequence compositeSequence)
        {
            Deploy = new RemoteDeploymentBuilder(compositeSequence);
            ExecuteRemote = new RemoteExecutionBuilder(compositeSequence);
        }

        public IOfferRemoteDeployment Deploy { get; private set; }
        public IOfferRemoteExecution ExecuteRemote { get; private set; }

    }
}