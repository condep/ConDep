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
        private readonly List<object> _sequence = new List<object>();
 
        public CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);

            if (operation is IRequireRemotePowerShellScripts)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScripts)operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence, new WebDeployHandler()));
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
                scriptOp.Configure(new RemoteCompositeBuilder(sequence, new WebDeployHandler()));
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

        public IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options)
        {
            try
            {
                Logger.LogSectionStart("Infrastructure");
                foreach (var element in _sequence)
                {
                    if (element is IOperateRemote)
                    {
                        ((IOperateRemote)element).Execute(server, status, options);
                        if (status.HasErrors)
                            return status;
                    }
                    else if (element is CompositeSequence)
                    {
                        ((CompositeSequence)element).Execute(server, status, options);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    if (status.HasErrors)
                        return status;
                }
            }
            finally
            {
                Logger.LogSectionEnd("Infrastructure");
            }

            return status;
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
    }
}