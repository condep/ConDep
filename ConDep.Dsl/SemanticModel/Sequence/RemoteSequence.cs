using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using System.Linq;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.PowerShellScript;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    //Todo: Could need some refactoring...
    public class RemoteSequence : IManageRemoteSequence, IExecute
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly PreOpsSequence _preOpsSequence;
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ILoadBalance _loadBalancer;
        private readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();

        public RemoteSequence(IManageInfrastructureSequence infrastructureSequence, PreOpsSequence preOpsSequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer)
        {
            _infrastructureSequence = infrastructureSequence;
            _preOpsSequence = preOpsSequence;
            _servers = servers;
            _loadBalancer = loadBalancer;
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

        public void Execute(IReportStatus status, ConDepSettings settings)
        {
            switch (_loadBalancer.Mode)
            {
                case LbMode.Sticky:
                    ExecuteWithSticky(settings, status);
                    return;
                case LbMode.RoundRobin:
                    ExecuteWithRoundRobin(settings, status);
                    return;
                default:
                    throw new ConDepLoadBalancerException(string.Format("Load Balancer mode [{0}] not supported.",
                                                                    _loadBalancer.Mode));
            }
        }

        public string Name { get { return "Remote Operations"; } }

        private void ExecuteWithRoundRobin(ConDepSettings settings, IReportStatus status)
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

                if (status.HasErrors)
                    return;
            }
        }

        private void ExecuteWithSticky(ConDepSettings settings, IReportStatus status)
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

                if (status.HasErrors)
                    return;

            }
        }

        private void TurnRoundRobinServersAround(ILoadBalance loadBalancer, IEnumerable<ServerConfig> servers, int roundRobinMaxOfflineServers, ServerConfig testServer, IReportStatus status)
        {
            if(testServer != null)
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

        private void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings, ILoadBalance loadBalancer, bool bringServerOfflineBeforeExecution, bool bringServerOnlineAfterExecution)
        {
            var errorDuringLoadBalancing = false;
            try
            {
                Logger.LogSectionStart(server.Name);

                if (bringServerOfflineBeforeExecution)
                {
                    Logger.Info(string.Format("Taking server [{0}] offline in load balancer.", server.Name));
                    loadBalancer.BringOffline(server.Name, server.LoadBalancerFarm,
                                                LoadBalancerSuspendMethod.Suspend, status);
                }

                ExecuteOnServer(server, status, settings);
                return;
            }
            catch
            {
                errorDuringLoadBalancing = true;
                throw;
            }
            finally
            {
                try
                {
                    if (bringServerOnlineAfterExecution && !status.HasErrors && !errorDuringLoadBalancing)
                    {
                        Logger.Info(string.Format("Taking server [{0}] online in load balancer.", server.Name));
                        loadBalancer.BringOnline(server.Name, server.LoadBalancerFarm, status);
                    }
                }
                finally
                {
                    Logger.LogSectionEnd(server.Name);
                }
            }
        }

        private void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _preOpsSequence.Execute(server, status, settings);
            if (status.HasErrors)
                return;

            _infrastructureSequence.Execute(server, status, settings);
            if (status.HasErrors)
                return;

            try
            {
                Logger.LogSectionStart("Deployment");
                foreach (var element in _sequence)
                {
                    element.Execute(server, status, settings);

                    if (status.HasErrors)
                        return;
                }
            }
            finally
            {
                Logger.LogSectionEnd("Deployment");
            }
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);

            if (operation is IRequireRemotePowerShellScripts)
            {
                var scriptOp = new PowerShellScriptDeployOperation(((IRequireRemotePowerShellScripts)operation).ScriptPaths);
                scriptOp.Configure(new RemoteCompositeBuilder(sequence));
            }

            _sequence.Add(sequence);
            return sequence;
        }

        public bool IsValid(Notification notification)
        {
            var isInfrastractureValid = _infrastructureSequence.IsValid(notification);
            var isRemoteOpValid = _sequence.OfType<IOperateRemote>().All(x => x.IsValid(notification));
            var isCompositeSeqValid = _sequence.OfType<CompositeSequence>().All(x => x.IsValid(notification));

            return isInfrastractureValid && isRemoteOpValid && isCompositeSeqValid;

        }
    }
}
