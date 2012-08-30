using System.Collections.Generic;
using ConDep.Dsl.LoadBalancer;

namespace ConDep.Dsl
{
    public class ConDepEnvironmentSettings
    {
        private readonly string _environment;
        private readonly LoadBalancerSettings _loadBalancer = new LoadBalancerSettings();
        private readonly DeploymentUser _deploymentUser = new DeploymentUser();
        private readonly List<DeploymentServer> _servers = new List<DeploymentServer>();

        public ConDepEnvironmentSettings(string environment)
        {
            _environment = environment;
        }

        public string Name
        {
            get { return _environment; }
        }

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