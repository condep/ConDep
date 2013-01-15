using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using System.Linq;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.PowerShellScript;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class RemoteSequence : IManageRemoteSequence
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly List<object> _sequence = new List<object>();

        public RemoteSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers)
        {
            _infrastructureSequence = infrastructureSequence;
            _servers = servers;
        }

        public void Add(IOperateRemote operation)
        {
            _sequence.Add(operation);
        }

        public IReportStatus Execute(IReportStatus status, ConDepOptions options)
        {
            foreach (var server in _servers)
            {
                try
                {
                    Logger.LogSectionStart(server.Name);

                    if (!options.WebDeployExist)
                    {
                        using (new WebDeployDeployer(server))
                        {
                            ExecuteOnServer(server, status, options);
                        }
                    }
                    else
                    {
                        ExecuteOnServer(server, status, options);
                    }
                }
                finally
                {
                    Logger.LogSectionEnd(server.Name);
                }
            }
            return status;
        }

        private IReportStatus ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepOptions options)
        {
            _infrastructureSequence.Execute(server, status, options);

            if (status.HasErrors)
                return status;

            foreach (var element in _sequence)
            {
                if (element.GetType().IsAssignableFrom(typeof(IRequireRemotePowerShellScript)))
                {
                    var scriptPaths = ((IRequireRemotePowerShellScript)element).ScriptPaths;
                    RemotePowerShellScripts.Add(scriptPaths);
                    //DeployPowerShellScripts(scriptPaths);
                }

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
            return status;
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);

            if (operation is IRequireRemotePowerShellScript)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScript)operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence, new WebDeployHandler()));
            }

            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsValid(Notification notification)
        {
            var isInfrastractureValid = _infrastructureSequence.IsvValid(notification);
            var isRemoteOpValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompositeSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isInfrastractureValid && isRemoteOpValid && isCompositeSeqValid;

        }
    }
}