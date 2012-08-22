using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Core
{
    public class InfrastructureWebSiteOptions : IProvideForInfrastrucutreWebSite
    {
        private readonly ISetupWebDeploy _webDeploySetup;
        private readonly WebSiteInfrastructureProvider _webSiteInfrastructureProvider;

        public InfrastructureWebSiteOptions(ISetupWebDeploy webDeploySetup, WebSiteInfrastructureProvider webSiteInfrastructureProvider)
        {
            _webDeploySetup = webDeploySetup;
            _webSiteInfrastructureProvider = webSiteInfrastructureProvider;
        }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }

        public string WebSiteName
        {
            get { return _webSiteInfrastructureProvider.WebSiteName; }
        }

        public string AppPoolName
        {
            get { return _webSiteInfrastructureProvider.AppPoolName; }
            set { _webSiteInfrastructureProvider.AppPoolName = value; }
        }
    }
}