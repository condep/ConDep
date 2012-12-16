using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class InfrastructureSequence : IManageInfrastructureSequence
    {
        private readonly List<CompositeSequence> _sequence = new List<CompositeSequence>();
        
        public CompositeSequence NewCompositeSequence(string compositeName)
        {
            var sequence = new CompositeSequence(compositeName);
            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsvValid(Notification notification)
        {
            return _sequence.All(x => x.IsValid(notification));
        }

        public IReportStatus Execute(ServerConfig server, IReportStatus status)
        {
            try
            {
                Logger.LogSectionStart("Infrastructure");
                foreach (var element in _sequence)
                {
                    element.Execute(server, status);
                    if (status.HasErrors)
                        return status;
                }
            }
            finally
            {
                Logger.LogSectionEnd("Infrastructure");
            }

            return status;
        }
    }
}