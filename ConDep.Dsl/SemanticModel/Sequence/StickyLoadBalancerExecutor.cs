using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class StickyLoadBalancerExecutor : LoadBalancerExecutorBase
    {
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ILoadBalance _loadBalancer;

        public StickyLoadBalancerExecutor(IManageInfrastructureSequence infrastructureSequence, List<IExecuteOnServer> sequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer)
            : base(infrastructureSequence, sequence)
        {
            _servers = servers;
            _loadBalancer = loadBalancer;
        }

        public override void Execute(IReportStatus status, ConDepSettings settings)
        {
            var servers = _servers.ToList();
            ServerConfig manuelTestServer;

            if (settings.Options.StopAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                ExecuteOnServer(manuelTestServer, status, settings, _loadBalancer, true, false);
                return;
            }

            if (settings.Options.ContinueAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                _loadBalancer.BringOnline(manuelTestServer.Name, manuelTestServer.LoadBalancerFarm, status);
                servers.Remove(manuelTestServer);
            }

            foreach (var server in servers)
            {
                ExecuteOnServer(server, status, settings, _loadBalancer, true, true);
            }
        }
    }
}