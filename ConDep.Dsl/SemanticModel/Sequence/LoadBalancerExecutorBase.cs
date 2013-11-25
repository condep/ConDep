using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public abstract class LoadBalancerExecutorBase
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly List<IExecuteOnServer> _sequence;

        protected LoadBalancerExecutorBase(IManageInfrastructureSequence infrastructureSequence, List<IExecuteOnServer> sequence)
        {
            _infrastructureSequence = infrastructureSequence;
            _sequence = sequence;
        }

        public abstract void Execute(IReportStatus status, ConDepSettings settings);

        protected void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings, ILoadBalance loadBalancer, bool bringServerOfflineBeforeExecution, bool bringServerOnlineAfterExecution)
        {
            var errorDuringLoadBalancing = false;

            Logger.WithLogSection(server.Name, () =>
                {
                    try
                    {
                        if (bringServerOfflineBeforeExecution)
                        {
                            Logger.Info(string.Format("Taking server [{0}] offline in load balancer.", server.Name));
                            loadBalancer.BringOffline(server.Name, server.LoadBalancerFarm,
                                                      LoadBalancerSuspendMethod.Suspend, status);
                        }

                        ExecuteOnServer(server, status, settings);
                    }
                    catch
                    {
                        errorDuringLoadBalancing = true;
                        throw;
                    }
                    finally
                    {
                        //&& !status.HasErrors
                        if (bringServerOnlineAfterExecution && !errorDuringLoadBalancing)
                        {
                            Logger.Info(string.Format("Taking server [{0}] online in load balancer.", server.Name));
                            loadBalancer.BringOnline(server.Name, server.LoadBalancerFarm, status);
                        }
                    }
                });

        }

        protected virtual void ExecuteOnServer(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _infrastructureSequence.Execute(server, status, settings);

            Logger.WithLogSection("Deployment", () =>
                {
                    foreach (var element in _sequence)
                    {
                        IExecuteOnServer elementToExecute = element;
                        if (element is CompositeSequence)
                            elementToExecute.Execute(server, status, settings);
                        else
                            Logger.WithLogSection(element.Name, () => elementToExecute.Execute(server, status, settings));
                    }
                });
        }


    }
}