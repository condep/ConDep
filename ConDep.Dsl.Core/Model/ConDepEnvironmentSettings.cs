using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    public class ConDepEnvironmentSettings
    {
        private readonly LoadBalancerSettings _loadBalancer = new LoadBalancerSettings();
        private readonly DeploymentUser _deploymentUser = new DeploymentUser();
        private readonly List<DeploymentServer> _servers = new List<DeploymentServer>();

        public LoadBalancerSettings LoadBalancer
        {
            get {
                return _loadBalancer;
            }
        }

        public DeploymentUser DeploymentUser
        {
            get {
                return _deploymentUser;
            }
        }

        public List<DeploymentServer> Servers
        {
            get {
                return _servers;
            }
        }
    }
}