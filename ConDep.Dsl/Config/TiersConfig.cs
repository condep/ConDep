using System;
using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class TiersConfig
    {
        public string Name { get; set; }
        public IList<ServerConfig> Servers { get; set; }
        public LoadBalancerConfig LoadBalancer { get; set; }
    }
}