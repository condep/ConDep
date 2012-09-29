using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.LoadBalancer
{
    public class DefaulLoadBalancer : ILoadBalance
    {
        public void BringOffline(string serverName, WebDeploymentStatus webDeploymentStatus)
        {
            Logger.Warn("Warning: No load balancer is used. If this is not by intention, make sure you configure a provider for load balancing.");
        }

        public void BringOnline(string serverName, WebDeploymentStatus webDeploymentStatus)
        {
            Logger.Warn("Warning: No load balancer is used. If this is not by intention, make sure you configure a provider for load balancing.");
        }
    }
}