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
    public class CompositeSequence : IManageRemoteSequence, IExecuteOnServer
    {
        private readonly string _compositeName;
        private readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();

        public CompositeSequence(string compositeName)
        {
            _compositeName = compositeName;
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

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection(_compositeName, () =>
                {
                    foreach (var element in _sequence)
                    {
                        element.Execute(server, status, settings);
                        //if (status.HasErrors)
                        //    return;
                    }
                });
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var opName = operation.Name;
            var sequence = new CompositeSequence(opName);

            if (operation is IRequireRemotePowerShellScripts)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScripts) operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence));
            }

            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsValid(Notification notification)
        {
            var isRemoteOpsValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isCompSeqValid && isRemoteOpsValid;
        }
    }
}