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
    public class RemoteSequence : IManageRemoteSequence
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ILoadBalance _loadBalancer;
        private readonly List<object> _sequence = new List<object>();

        public RemoteSequence(IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers, ILoadBalance loadBalancer)
        {
            _infrastructureSequence = infrastructureSequence;
            _servers = servers;
            _loadBalancer = loadBalancer;
        }

        public void Add(IOperateRemote operation)
        {
            _sequence.Add(operation);
        }

        public IReportStatus Execute(IReportStatus status, ConDepOptions options)
        {
            switch (_loadBalancer.Mode)
            {
                case LbMode.Sticky:
                    return ExecuteWithSticky(options, status);
                case LbMode.RoundRobin:
                    return ExecuteWithRoundRobin(options, status);
                default:
                    throw new NotSupportedException(string.Format("Load Balancer mode [{0}] not supported.", _loadBalancer.Mode));
            }
        }

        private IReportStatus ExecuteWithRoundRobin(ConDepOptions options, IReportStatus status)
        {
            var servers = _servers.ToList();
            var roundRobinMaxOfflineServers = (int)Math.Ceiling(((double)servers.Count) / 2);
            ServerConfig manuelTestServer = null;

            if (options.StopAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                return ExecuteOnServer(manuelTestServer, status, options, _loadBalancer, true, false);
            }

            if (options.ContinueAfterMarkedServer)
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
                return ExecuteOnServer(servers.First(), status, options, _loadBalancer, true, true);
            }

            for (int execCount = 0; execCount < servers.Count; execCount++)
            {
                if (execCount == roundRobinMaxOfflineServers - (manuelTestServer == null ? 0 : 1))
                {
                    TurnRoundRobinServersAround(_loadBalancer, servers, roundRobinMaxOfflineServers, manuelTestServer, status);
                }

                bool bringOnline = !(roundRobinMaxOfflineServers - (manuelTestServer == null ? 0 : 1) > execCount);
                ExecuteOnServer(servers[execCount], status, options, _loadBalancer, !bringOnline, bringOnline);
            }

            return status;
        }

        private IReportStatus ExecuteWithSticky(ConDepOptions options, IReportStatus status)
        {
            var servers = _servers.ToList();
            ServerConfig manuelTestServer;

            if (options.StopAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                return ExecuteOnServer(manuelTestServer, status, options, _loadBalancer, true, false);
            }

            if (options.ContinueAfterMarkedServer)
            {
                manuelTestServer = servers.SingleOrDefault(x => x.StopServer) ?? servers.First();
                _loadBalancer.BringOnline(manuelTestServer.Name, manuelTestServer.LoadBalancerFarm, status);
                servers.Remove(manuelTestServer);
            }

            foreach (var server in servers)
            {
                ExecuteOnServer(server, status, options, _loadBalancer, true, true);
            }
            return status;
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

        private IReportStatus ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepOptions options, ILoadBalance loadBalancer, bool bringServerOfflineBeforeExecution, bool bringServerOnlineAfterExecution)
        {
            try
            {
                Logger.LogSectionStart(server.Name);

                if (bringServerOfflineBeforeExecution)
                {
                    Logger.Info(string.Format("Taking server [{0}] offline in load balancer.", server.Name));
                    loadBalancer.BringOffline(server.Name, server.LoadBalancerFarm, LoadBalancerSuspendMethod.Suspend, status);
                }

                if (options.WebDeployExist)
                {
                    return ExecuteOnServer(server, status, options);
                }
                else
                {
                    using (new WebDeployDeployer(server))
                    {
                        return ExecuteOnServer(server, status, options);
                    }
                }
            }
            finally
            {
                try
                {
                    if (bringServerOnlineAfterExecution)
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