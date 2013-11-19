using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class RemoteConditionalSequence : RemoteSequence
    {
        private readonly Predicate<ServerInfo> _condition;
        private readonly bool _expectedConditionResult;

        public RemoteConditionalSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer, Predicate<ServerInfo> condition, bool expectedConditionResult)
            : base(infrastructureSequence, servers, loadBalancer)
        {
            _condition = condition;
            _expectedConditionResult = expectedConditionResult;
        }

        protected override void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _infrastructureSequence.Execute(server, status, settings);

            Logger.WithLogSection("Deployment", () =>
                {
                    if (_condition(server.GetServerInfo()) == _expectedConditionResult)
                    {
                        foreach (var element in _sequence)
                        {
                            IExecuteOnServer elementToExecute = element;
                            if (element is CompositeSequence)
                                elementToExecute.Execute(server, status, settings);
                            else
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
            get { return "Conditional Remote Operation"; }
        }
    }
}