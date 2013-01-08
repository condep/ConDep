using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.PowerShellScript;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class CompositeSequence : IManageRemoteSequence
    {
        private readonly string _compositeName;
        private readonly List<object> _sequence = new List<object>();

        public CompositeSequence(string compositeName)
        {
            _compositeName = compositeName;
        }

        public string CompositeName
        {
            get { return _compositeName; }
        }

        public void Add(IOperateRemote operation)
        {
            _sequence.Add(operation);
        }

        public IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options)
        {
            try
            {
                Logger.LogSectionStart(_compositeName);
                foreach (var element in _sequence)
                {
                    if (element is CompositeSequence)
                    {
                        ((CompositeSequence)element).Execute(server, status, options);
                    }
                    else if (element is IOperateRemote)
                    {
                        ((IOperateRemote)element).Execute(server, status, options);
                    }

                    if (status.HasErrors)
                        return status;
                }
            }
            finally
            {
                Logger.LogSectionEnd(_compositeName);
            }
            return status;
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var opName = operation.Name;
            var sequence = new CompositeSequence(opName);

            if (operation is IRequireRemotePowerShellScript)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScript) operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence, new WebDeployHandler()));
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