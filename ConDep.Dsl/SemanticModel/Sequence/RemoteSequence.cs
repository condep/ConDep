using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using System.Linq;
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

        public IReportStatus Execute(IReportStatus status)
        {
            foreach (var server in _servers)
            {
                using(new WebDeployDeployer(server))
                {
                    try
                    {
                        Logger.LogSectionStart(server.Name);
                        _infrastructureSequence.Execute(server, status);
                        if (status.HasErrors)
                            return status;

                        foreach (var element in _sequence)
                        {
                            if (element is IOperateRemote)
                            {
                                ((IOperateRemote)element).Execute(server, status);
                                if (status.HasErrors)
                                    return status;
                            }
                            else if (element is CompositeSequence)
                            {
                                ((CompositeSequence)element).Execute(server, status);
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
                        Logger.LogSectionEnd(server.Name);
                    }
                }
            }
            return status;
        }

        public CompositeSequence NewCompositeSequence(string compositeName)
        {
            var sequence = new CompositeSequence(compositeName);
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