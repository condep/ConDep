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
        private readonly ILoadBalance _loadBalancer;
        private readonly List<IOperateRemote> _sequence = new List<IOperateRemote>();

        public RemoteSequenceManager(IEnumerable<ServerConfig> servers)
        {
            _servers = servers;
            _loadBalancer = TinyIoC.TinyIoCContainer.Current.Resolve<ILoadBalance>();
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
                    _loadBalancer.BringOffline(server.Name, LoadBalancerSuspendMethod.Suspend, status);
                    if (status.HasErrors)
                        return status;

                    Logger.LogSectionStart(server.Name);
                    foreach (var element in _sequence)
                    {
                        element.Execute(server, status);
                        if (status.HasErrors)
                            return status;
                    }

                    _loadBalancer.BringOnline(server.Name, status);
                    if (status.HasErrors)
                        return status;
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