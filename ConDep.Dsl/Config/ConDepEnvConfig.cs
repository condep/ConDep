using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class ConDepEnvConfig
    {
        public string EnvironmentName { get; set; }
        public LoadBalancerConfig LoadBalancer { get; set; }
        public IList<ServerConfig> Servers { get; set; }
        public IList<TiersConfig> Tiers { get; set; }
        public DeploymentUserConfig DeploymentUser { get; set; }
        public IList<CustomProviderConfig> CustomProviderConfig { get; set; }
    }
}