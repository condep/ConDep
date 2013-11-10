using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.PowerShellScript;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class InfrastructureSequence : IManageInfrastructureSequence
    {
        private readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();
 
        public CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);

            if (operation is IRequireRemotePowerShellScripts)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScripts)operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence));
            }

            _sequence.Add(sequence);
            return sequence;
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);
            if (operation is IRequireRemotePowerShellScripts)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScripts)operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence));
            }
            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsValid(Notification notification)
        {
            var isRemoteOpValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompositeSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isRemoteOpValid && isCompositeSeqValid;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection("Infrastructure", () =>
                {
                    foreach (var element in _sequence)
                    {
                        IExecuteOnServer elementToExecute = element;
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

        public string Name { get { return "Infrastructure"; } }
    }
}