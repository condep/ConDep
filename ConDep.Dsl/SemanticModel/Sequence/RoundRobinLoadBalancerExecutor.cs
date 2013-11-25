using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class RoundRobinLoadBalancerExecutor : LoadBalancerExecutorBase
    {
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ILoadBalance _loadBalancer;

        public RoundRobinLoadBalancerExecutor(IManageInfrastructureSequence infrastructureSequence, List<IExecuteOnServer> sequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer) : base(infrastructureSequence, sequence)
        {
            _servers = servers;
            _loadBalancer = loadBalancer;
        }

        public override void Execute(IReportStatus status, ConDepSettings settings)
        {
            var servers = _servers.ToList();
            var roundRobinMaxOfflineServers = (int)Math.Ceiling(((double)servers.Count) / 2);
            ServerConfig manuelTestServer = null;

            if (settings.Options.StopAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                ExecuteOnServer(manuelTestServer, status, settings, _loadBalancer, true, false);
                return;
            }

            if (settings.Options.ContinueAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                servers.Remove(manuelTestServer);

                if (roundRobinMaxOfflineServers == 1)
                {
                    _loadBalancer.BringOnline(manuelTestServer.Name, manuelTestServer.LoadBalancerFarm, status);
                }
            }

            if (servers.Count == 1)
            {
                ExecuteOnServer(servers.First(), status, settings, _loadBalancer, true, true);
                return;
            }

            for (int execCount = 0; execCount < servers.Count; execCount++)
            {
                if (execCount == roundRobinMaxOfflineServers - (manuelTestServer == null ? 0 : 1))
                {
                    TurnRoundRobinServersAround(_loadBalancer, servers, roundRobinMaxOfflineServers, manuelTestServer, status);
                }

                bool bringOnline = !(roundRobinMaxOfflineServers - (manuelTestServer == null ? 0 : 1) > execCount);
                ExecuteOnServer(servers[execCount], status, settings, _loadBalancer, !bringOnline, bringOnline);

                //if (status.HasErrors)
                //    return;
            }
        }

        private void TurnRoundRobinServersAround(ILoadBalance loadBalancer, IEnumerable<ServerConfig> servers, int roundRobinMaxOfflineServers, ServerConfig testServer, IReportStatus status)
        {
            if (testServer != null)
            {
                loadBalancer.BringOnline(testServer.Name, testServer.LoadBalancerFarm, status);
            }
            var numberOfServers = roundRobinMaxOfflineServers - (testServer == null ? 0 : 1);

            var serversToBringOnline = servers.Take(numberOfServers);
            foreach (var server in serversToBringOnline)
            {
                loadBalancer.BringOnline(server.Name, server.LoadBalancerFarm, status);
            }
            var serversToBringOffline = servers.Skip(numberOfServers);
            foreach (var server in serversToBringOffline)
            {
                loadBalancer.BringOffline(server.Name, server.LoadBalancerFarm, LoadBalancerSuspendMethod.Suspend, status);
            }
        }

    }
}