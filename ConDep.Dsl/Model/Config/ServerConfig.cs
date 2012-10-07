using System.Collections.Generic;

namespace ConDep.Dsl.Model.Config
{
    public class ServerConfig
    {
        public string Name { get; set; }
        public IList<WebSiteConfig> WebSites { get; set; }
        public DeploymentUserConfig DeploymentUser { get; set; }
    }
}