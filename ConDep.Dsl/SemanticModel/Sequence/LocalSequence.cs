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
        private readonly ILoadBalance _loadBalancer;
        private readonly List<object> _sequence = new List<object>();

        public LocalSequence(string name, ILoadBalance loadBalancer)
        {
            _name = name;
            _loadBalancer = loadBalancer;
        }

        public void Add(LocalOperation operation, bool addFirst = false)
        {
            if(addFirst)
            {
                _sequence.Insert(0, operation);
            }
            else
            {
                _sequence.Add(operation);
            }
        }

        public RemoteSequence NewRemoteSequence(IManageInfrastructureSequence infrastructureSequence, PreOpsSequence preOpsSequence, IEnumerable<ServerConfig> servers)
        {
            var sequence = new RemoteSequence(infrastructureSequence, preOpsSequence, servers, _loadBalancer);
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
                        ((RemoteSequence)element).Execute(status, options);
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