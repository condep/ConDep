using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class ExecutionSequenceManager
    {
        private readonly List<LocalSequence> _sequence = new List<LocalSequence>();

        public void Add(LocalSequence localOperation)
        {
            _sequence.Add(localOperation);
        }

        public LocalSequence NewLocalSequence(string name)
        {
            var sequence = new LocalSequence(name);
            _sequence.Add(sequence);
            return sequence;
        }

        public IReportStatus Execute(IReportStatus status, ConDepConfig config, ConDepOptions options)
        {
            foreach (var localSequence in _sequence)
            {
                localSequence.Execute(status, config, options);
                if (status.HasErrors)
                    return status;
            }
            return status;
        }

        public bool IsValid(Notification notification)
        {
            return _sequence.All(x => x.IsValid(notification));
        }
    }
}