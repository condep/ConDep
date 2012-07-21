using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    public class DeploymentServer
    {
        private readonly string _serverName;
        private readonly List<ConDepWebSiteSettings> _webSites = new List<ConDepWebSiteSettings>();

        public DeploymentServer(string serverName)
        {
            _serverName = serverName;
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
    }
}