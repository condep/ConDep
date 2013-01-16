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
    public class InfrastructureSequence : IManageInfrastructureSequence
    {
        private readonly List<CompositeSequence> _sequence = new List<CompositeSequence>();
        
        public CompositeSequence NewCompositeSequence(RemoteCompositeInfrastructureOperation operation)
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

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);
            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsvValid(Notification notification)
        {
            return _sequence.All(x => x.IsValid(notification));
        }

        public IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options)
        {
            try
            {
                Logger.LogSectionStart("Infrastructure");
                foreach (var element in _sequence)
                {
                    element.Execute(server, status, options);
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
    }
}