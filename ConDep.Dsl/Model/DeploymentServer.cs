using System.Collections.Generic;

namespace ConDep.Dsl
{
    public class DeploymentServer
    {
        private readonly string _serverName;
        private readonly DeploymentUser _user;
        private readonly List<ConDepWebSiteSettings> _webSites = new List<ConDepWebSiteSettings>();

        public DeploymentServer(string serverName, DeploymentUser user)
        {
            _serverName = serverName;
            _user = user;
        }

        public string ServerName
        {
            get { return _serverName; }
        }

        public List<ConDepWebSiteSettings> WebSites
        {
            get {
                return _webSites;
            }
        }

        public DeploymentUser User
        {
            get { return _user; }
        }
    }
}