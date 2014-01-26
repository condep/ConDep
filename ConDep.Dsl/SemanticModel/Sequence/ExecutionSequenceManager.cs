using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class ExecutionSequenceManager
    {
        private readonly ILoadBalance _loadBalancer;
        internal readonly List<LocalSequence> _sequence = new List<LocalSequence>();

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

        public void Execute(IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            foreach (var localSequence in _sequence)
            {
                token.ThrowIfCancellationRequested();

                LocalSequence sequence = localSequence;
                Logger.WithLogSection(localSequence.Name, () => sequence.Execute(status, settings, token));
            }
        }

        public bool IsValid(Notification notification)
        {
            return _sequence.All(x => x.IsValid(notification));
        }

        public void DryRun()
        {
            foreach (var item in _sequence)
            {
                Logger.Info(item.Name);
                item.DryRun();
            }
        }
    }
}