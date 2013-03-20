using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class LocalSequence : IManageSequence<LocalOperation>, IExecute
    {
        private readonly string _name;
        private readonly ILoadBalance _loadBalancer;
        private readonly List<IExecute> _sequence = new List<IExecute>();

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

        public void Execute(IReportStatus status, ConDepSettings settings)
        {
            try
            {
                Logger.LogSectionStart(_name);
                foreach (var element in _sequence)
                {
                    element.Execute(status, settings);
                    //if (element is LocalOperation)
                    //{
                    //    Logger.LogSectionStart(element.GetType().Name);
                    //    ((LocalOperation)element).Execute(status, config, options);
                    //    Logger.LogSectionEnd(element.GetType().Name);
                    //}
                    //else if (element is RemoteSequence)
                    //{
                    //    ((RemoteSequence)element).Execute(status, options);
                    //}
                    //else
                    //{
                    //    throw new NotSupportedException();
                    //}

                    if (status.HasErrors)
                        return;
                }
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