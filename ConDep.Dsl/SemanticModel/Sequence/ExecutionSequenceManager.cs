using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class ExecutionSequenceManager
    {
        private readonly ILoadBalance _loadBalancer;
        private readonly List<LocalSequence> _sequence = new List<LocalSequence>();

        public ExecutionSequenceManager(ILoadBalance loadBalancer)
        {
            _loadBalancer = loadBalancer;
        }

        public LocalSequence NewLocalSequence(string name)
        {
            var sequence = new LocalSequence(name, _loadBalancer);
            _sequence.Add(sequence);
            return sequence;
        }

        public void Execute(IReportStatus status, ConDepSettings settings)
        {
            foreach (var localSequence in _sequence)
            {
                localSequence.Execute(status, settings);
                //if (status.HasErrors)
                //    return;
            }
        }

        public bool IsValid(Notification notification)
        {
            return _sequence.All(x => x.IsValid(notification));
        }
    }
}