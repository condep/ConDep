using System.Collections.Generic;

namespace ConDep.Dsl.Model.Config
{
    public class ConDepConfig
    {
        public LoadBalancerConfig LoadBalancer { get; set; }
        public IList<ServerConfig> Servers { get; set; }
        public DeploymentUserConfig DeploymentUser { get; set; }
        public IList<CustomProviderConfig> CustomProviderConfig { get; set; }
    }
}