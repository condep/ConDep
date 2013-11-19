using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class ConditionalInfrastructureSequence : InfrastructureSequence
    {
        private Predicate<ServerInfo> _condition;
        private bool _expectedConditionResult;

        public ConditionalInfrastructureSequence(string name, Predicate<ServerInfo> condition, bool expectedConditionResult)
        {
            _condition = condition;
            _expectedConditionResult = expectedConditionResult;
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection(Name, () =>
                {
                    if (_condition(server.GetServerInfo()) == _expectedConditionResult)
                    {
                        foreach (var element in _sequence)
                        {
                            IExecuteOnServer elementToExecute = element;
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings));
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
            get
            {
                return "Conditional Infrastructure";
            }
        }
    }
}