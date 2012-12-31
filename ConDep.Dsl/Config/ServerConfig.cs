using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class ServerConfig
    {
        private DeploymentUserConfig _deploymentUser;

        public string Name { get; set; }
        public IList<WebSiteConfig> WebSites { get; set; }
        public DeploymentUserConfig DeploymentUser 
        { 
            get { return _deploymentUser ?? (_deploymentUser = new DeploymentUserConfig()); }
            set { _deploymentUser = value; }
        }

        public string WebDeployAgentUrl { get; set; }
    }
}