using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Builders
{
    public class RemoteCompositeBuilder : IOfferRemoteComposition
    {
        private readonly CompositeSequence _compositeSequence;

        public RemoteCompositeBuilder(CompositeSequence compositeSequence)
        {
            _compositeSequence = compositeSequence;
            Deploy = new RemoteDeploymentBuilder(compositeSequence);
            ExecuteRemote = new RemoteExecutionBuilder(compositeSequence);
        }

        public IOfferRemoteDeployment Deploy { get; private set; }
        public IOfferRemoteExecution ExecuteRemote { get; private set; }

        public IOfferRemoteComposition OnlyIf(Predicate<ServerInfo> condition)
        {
            var sequence = _compositeSequence.NewConditionalCompositeSequence(condition);
            return new RemoteCompositeBuilder(sequence);
        }
    }
}