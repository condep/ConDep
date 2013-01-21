using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class LocalSequence : IManageSequence<LocalOperation>
    {
        private readonly string _name;
        private readonly List<object> _sequence = new List<object>();

        public LocalSequence(string name)
        {
            _name = name;
        }

        public void Add(LocalOperation operation)
        {
            _sequence.Add(operation);
        }

        public RemoteSequence NewRemoteSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers)
        {
            var sequence = new RemoteSequence(infrastructureSequence, servers);
            _sequence.Add(sequence);
            return sequence;
        }

        public IReportStatus Execute(IReportStatus status, ConDepConfig config, ConDepOptions options)
        {
            try
            {
                Logger.LogSectionStart(_name);
                foreach (var element in _sequence)
                {
                    if (element is LocalOperation)
                    {
                        Logger.LogSectionStart(element.GetType().Name);
                        ((LocalOperation)element).Execute(status, config, options);
                        Logger.LogSectionEnd(element.GetType().Name);
                    }
                    else if (element is RemoteSequence)
                    {
                        return ((RemoteSequence)element).Execute(status, options);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    if (status.HasErrors)
                        return status;
                }
                return status;
                
            }
            finally
            {
                Logger.LogSectionEnd(_name);
            }
        }

        public bool IsValid(Notification notification)
        {
            var isLocalOpsValid = _sequence.OfType<LocalOperation>().All(x => x.IsValid(notification));
            var isRemoteSeqValid = _sequence.OfType<RemoteSequence>().All(x => x.IsValid(notification));
            return isLocalOpsValid && isRemoteSeqValid;
        }
    }
}