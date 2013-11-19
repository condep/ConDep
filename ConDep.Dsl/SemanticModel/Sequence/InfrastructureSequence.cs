using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class InfrastructureSequence : IManageInfrastructureSequence
    {
        protected readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();
        private SequenceFactory _sequenceFactory;

        public InfrastructureSequence()
        {
            _sequenceFactory = new SequenceFactory(_sequence);
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation)
        {
            return _sequenceFactory.NewCompositeSequence(operation);
        }

        public InfrastructureSequence NewConditionalInfrastructureSequence(Predicate<ServerInfo> condition)
        {
            var sequence = new ConditionalInfrastructureSequence(Name, condition, true);
            _sequence.Add(sequence);
            return sequence;
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            return _sequenceFactory.NewCompositeSequence(operation);
        }

        public CompositeSequence NewConditionalCompositeSequence(Predicate<ServerInfo> condition)
        {
            return _sequenceFactory.NewConditionalCompositeSequence(condition);
        }

        public InfrastructureSequence NewConditionalInfrastructureSequence(InfrastructureArtifact artifact, Predicate<ServerInfo> condition, bool expectedConditionResult)
        {
            return _sequenceFactory.NewConditionalInfrastructureSequence(artifact, condition, expectedConditionResult);
        }

        public bool IsValid(Notification notification)
        {
            var isRemoteOpValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompositeSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isRemoteOpValid && isCompositeSeqValid;
        }

        public virtual void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection("Infrastructure", () =>
                {
                    foreach (var element in _sequence)
                    {
                        IExecuteOnServer elementToExecute = element;
                        if(element is CompositeSequence || element is InfrastructureSequence)
                            elementToExecute.Execute(server, status, settings);
                        else
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings));
                    }
                });
        }

        public void Add(IOperateRemote operation, bool addFirst = false)
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

        public virtual string Name { get { return "Infrastructure"; } }
    }
}