using System;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class CompositeConditionalSequence : CompositeSequence
    {
        internal readonly Predicate<ServerInfo> _condition;
        private readonly bool _expectedConditionResult;

        public CompositeConditionalSequence(string name, Predicate<ServerInfo> condition, bool expectedConditionResult)
            : base(name)
        {
            _condition = condition;
            _expectedConditionResult = expectedConditionResult;
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            Logger.WithLogSection(Name, () =>
                {
                    if (_condition(server.GetServerInfo()) == _expectedConditionResult)
                    {
                        foreach (var element in _sequence)
                        {
                            IExecuteOnServer elementToExecute = element;
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings, token));
                        }
                    }
                    else
                    {
                        Logger.Info("Condition evaluated to false. Will not execute.");
                    }
                });
        }

        public override string Name
        {
            get { return "Conditional Operation"; }
        }
    }
}