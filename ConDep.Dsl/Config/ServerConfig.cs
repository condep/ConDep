using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class ServerConfig
    {
        private DeploymentUserConfig _deploymentUserRemote;
        private DeploymentUserConfig _deploymentUserLocal;

        public string Name { get; set; }
        public IList<WebSiteConfig> WebSites { get; set; }
        public DeploymentUserConfig DeploymentUserRemote 
        { 
            get { return _deploymentUserRemote ?? (_deploymentUserRemote = new DeploymentUserConfig()); }
            set { _deploymentUserRemote = value; }
        }

        public DeploymentUserConfig DeploymentUserLocal
        {
            get { return _deploymentUserLocal ?? (_deploymentUserLocal = new DeploymentUserConfig()); }
            set { _deploymentUserLocal = value; }
        }

        public string WebDeployAgentUrl { get; set; }
    }
}