using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConDep.Dsl.Config
{
    public class ServerConfig
    {
        private DeploymentUserConfig _deploymentUserRemote;
        private ServerInfo _serverInfo = new ServerInfo();

        public string Name { get; set; }
        public bool StopServer { get; set; }
        public IList<WebSiteConfig> WebSites { get; set; }
        public DeploymentUserConfig DeploymentUser 
        { 
            get { return _deploymentUserRemote ?? (_deploymentUserRemote = new DeploymentUserConfig()); }
            set { _deploymentUserRemote = value; }
        }

        public string LoadBalancerFarm { get; set; }

        public ServerInfo GetServerInfo()
        {
            return _serverInfo;
        }
    }
}