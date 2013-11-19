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
        internal readonly List<IExecute> _sequence = new List<IExecute>();

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

        public RemoteSequence NewRemoteSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers)
        {
            var sequence = new RemoteSequence(infrastructureSequence, servers, _loadBalancer);
            _sequence.Add(sequence);
            return sequence;
        }

        public RemoteSequence NewRemoteConditionalSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers, Predicate<ServerInfo> condition, bool expectedConditionResult)
        {
            var sequence = new RemoteConditionalSequence(infrastructureSequence, servers, _loadBalancer, condition, expectedConditionResult);
            _sequence.Add(sequence);
            return sequence;
        }

        public void Execute(IReportStatus status, ConDepSettings settings)
        {
            foreach (var element in _sequence)
            {
                IExecute internalElement = element;
                Logger.WithLogSection(internalElement.Name, () => internalElement.Execute(status, settings));
            }
        }

        public string Name { get { return _name; } }

        public bool IsValid(Notification notification)
        {
            var isLocalOpsValid = _sequence.OfType<LocalOperation>().All(x => x.IsValid(notification));
            var isRemoteSeqValid = _sequence.OfType<RemoteSequence>().All(x => x.IsValid(notification));
            return isLocalOpsValid && isRemoteSeqValid;
        }

        public void DryRun()
        {
            foreach (var item in _sequence)
            {
                item.DryRun();
            }
        }
    }
}