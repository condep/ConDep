using System;
using System.Collections.Generic;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using System.Linq;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.LoadBalancer;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    //Todo: Could need some refactoring...
    public class RemoteSequence : IManageRemoteSequence, IExecute
    {
        protected readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ILoadBalance _loadBalancer;
        internal readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();
        private readonly SequenceFactory _sequenceFactory;

        public RemoteSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer)
        {
            _infrastructureSequence = infrastructureSequence;
            _servers = servers;
            _loadBalancer = loadBalancer;
            _sequenceFactory = new SequenceFactory(_sequence);
        }

        public void Add(IOperateRemote operation, bool addFirst = false)
        {
            if (addFirst)
            {
                _sequence.Insert(0, operation);
            }
            else
            {
                _sequence.Add(operation);
            }
        }

        public virtual void Execute(IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            LoadBalancerExecutorBase lbExecutor;
 
            switch (_loadBalancer.Mode)
            {
                case LbMode.Sticky:
                    lbExecutor = new StickyLoadBalancerExecutor(_infrastructureSequence, _sequence, _servers, _loadBalancer);
                    break;
                    //ExecuteWithSticky(settings, status);
                    //return;
                case LbMode.RoundRobin:
                    lbExecutor = new RoundRobinLoadBalancerExecutor(_infrastructureSequence, _sequence, _servers, _loadBalancer);
                    break;
                    //ExecuteWithRoundRobin(settings, status);
                    //return;
                default:
                    throw new ConDepLoadBalancerException(string.Format("Load Balancer mode [{0}] not supported.",
                                                                    _loadBalancer.Mode));
            }

            lbExecutor.Execute(status, settings, token);
        }

        public virtual string Name { get { return "Remote Operations"; } }
        public void DryRun()
        {
            foreach (var item in _sequence)
            {
                Logger.Info(item.Name);
            }
        }

        protected virtual void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            _infrastructureSequence.Execute(server, status, settings, token);

            Logger.WithLogSection("Deployment", () =>
                {
                    foreach (var element in _sequence)
                    {
                        IExecuteOnServer elementToExecute = element;
                        if (element is CompositeSequence)
                            elementToExecute.Execute(server, status, settings, token);
                        else
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings, token));
                    }
                });
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            return _sequenceFactory.NewCompositeSequence(operation);
        }

        public CompositeSequence NewConditionalCompositeSequence(Predicate<ServerInfo> condition)
        {
            var sequence = new CompositeConditionalSequence(Name, condition, true);
            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsValid(Notification notification)
        {
            var isInfrastractureValid = _infrastructureSequence.IsValid(notification);
            var isRemoteOpValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompositeSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isInfrastractureValid && isRemoteOpValid && isCompositeSeqValid;

        }
    }
}
