using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public class WebSiteInfrastructureProviderOptions : ProviderOptions, IProvideForInfrastrucutreWebSite
    {
        private readonly WebSiteInfrastructureProvider _webSiteInfrastructureProvider;

        //Todo: Somehow this should probably get DeployentServer injected
        public WebSiteInfrastructureProviderOptions(WebSiteInfrastructureProvider webSiteInfrastructureProvider) : base(webSiteInfrastructureProvider.ChildProviders)
        {
            _webSiteInfrastructureProvider = webSiteInfrastructureProvider;
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