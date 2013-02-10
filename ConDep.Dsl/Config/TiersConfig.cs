using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class TiersConfig
    {
        public string Name { get; set; }
        public IList<ServerConfig> Servers { get; set; }
        public LoadBalancerConfig LoadBalancer { get; set; }
    }
}