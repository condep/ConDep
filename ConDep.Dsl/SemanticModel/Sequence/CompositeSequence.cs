using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class CompositeSequence : IManageRemoteSequence, IExecuteOnServer
    {
        private readonly string _compositeName;
        internal readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();
        private SequenceFactory _sequenceFactory;

        public CompositeSequence(string compositeName)
        {
            _compositeName = compositeName;
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

        public virtual void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection(_compositeName, () =>
                {
                    foreach (var element in _sequence)
                    {
                        IExecuteOnServer elementToExecute = element;
                        if (element is CompositeSequence)
                            elementToExecute.Execute(server, status, settings);
                        else
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings));

                    }
                });
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            return _sequenceFactory.NewCompositeSequence(operation);
        }

        public CompositeSequence NewConditionalCompositeSequence(Predicate<ServerInfo> condition)
        {
            return new CompositeConditionalSequence(Name, condition, true);
        }

        public bool IsValid(Notification notification)
        {
            var isRemoteOpsValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isCompSeqValid && isRemoteOpsValid;
        }

        public virtual string Name { get { return "Composite Operation"; } }
    }
}