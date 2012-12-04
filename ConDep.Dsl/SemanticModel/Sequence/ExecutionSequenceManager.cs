using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class ExecutionSequenceManager : IManageExecutionSequence
    {
        private readonly List<ISequenceElement> _sequence = new List<ISequenceElement>();

        public void Add(IManageRemoteSequence remoteSequence)
        {
            _sequence.Add(remoteSequence);
        }

        public void Add(IManageLocalSequence localSequence)
        {
            _sequence.Add(localSequence);
        }

        public void Add(LocalOperation operation)
        {
            _sequence.Add(new LocalOperationSequenceElement(operation));
        }

        public IManageLocalSequence NewLocalSequence()
        {
            var localSeq = new LocalSequenceManager();
            Add(localSeq);
            return localSeq;
        }

        public IManageRemoteSequence NewRemoteSequence(IEnumerable<ServerConfig> servers)
        {
            var remoteSeq = new RemoteSequenceManager(servers);
            Add(remoteSeq);
            return remoteSeq;
        }

        public bool IsValid(Notification notification)
        {
            return !_sequence.Any(x => x.IsValid(notification) == false);
        }

        public IReportStatus Execute(IReportStatus status)
        {
            foreach (var element in _sequence)
            {
                element.Execute(status);
                if (status.HasErrors)
                    return status;
            }
            return status;
        }

        public static IManageRemoteSequence GetSequenceFor(IOfferRemoteDeployment deployment)
        {
            return ((RemoteDeploymentBuilder) deployment).Sequence;
        }

        public static IManageRemoteSequence GetSequenceFor(IOfferRemoteExecution execution)
        {
            return ((RemoteExecutionBuilder)execution).Sequence;
        }

        public static IManageGeneralSequence GetSequenceFor(IOfferLocalOperations appOps)
        {
            return ((LocalOperationsBuilder)appOps).Sequence;
        }
    }
}