using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class RemoteSequenceManager : IManageRemoteSequence
    {
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly IEnumerable<InfrastructureArtifact> _infrastructures;
        private readonly ILoadBalance _loadBalancer;
        private readonly List<IOperateRemote> _sequence = new List<IOperateRemote>();

        public RemoteSequenceManager(IEnumerable<ServerConfig> servers, IEnumerable<InfrastructureArtifact> infrastructures) : this(servers, infrastructures, false)
        {
        }

        public RemoteSequenceManager(IEnumerable<ServerConfig> servers, IEnumerable<InfrastructureArtifact> infrastructures, bool noLoadBalancing)
        {
            _servers = servers;
            _infrastructures = infrastructures;

            if(!noLoadBalancing)
            {
                _loadBalancer = TinyIoC.TinyIoCContainer.Current.Resolve<ILoadBalance>();
            }
        }

        public void Add(IOperateRemote remoteOperation)
        {
            _sequence.Add(remoteOperation);
        }

        public IReportStatus Execute(IReportStatus status)
        {
            foreach(var server in _servers)
            {
                try
                {
                    if(_loadBalancer != null)
                    {
                        _loadBalancer.BringOffline(server.Name, LoadBalancerSuspendMethod.Suspend, status);
                        if (status.HasErrors)
                            return status;
                    }


                    Logger.LogSectionStart(server.Name);
                    foreach (var element in _sequence)
                    {
                        element.Execute(server, status);
                        if (status.HasErrors)
                            return status;
                    }

                    if(_loadBalancer != null)
                    {
                        _loadBalancer.BringOnline(server.Name, status);
                        if (status.HasErrors)
                            return status;
                    }
                }
                finally
                {
                    Logger.LogSectionEnd(server.Name);
                }
            }
            return status;
        }

        public bool IsValid(Notification notification)
        {
            return !_sequence.Any(x => x.IsValid(notification) == false);
        }
    }
}