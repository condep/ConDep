using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public class WebSiteDeploymentOptions : IProvideOptions<WebSiteDeploymentOptions>
    {
        private readonly WebSiteDeploymentProvider _webSiteDeploymentProvider;

        public WebSiteDeploymentOptions(WebSiteDeploymentProvider webSiteDeploymentProvider)
        {
            _webSiteDeploymentProvider = webSiteDeploymentProvider;
        }

        public WebSiteDeploymentExcludeOptions Exclude { get { return new WebSiteDeploymentExcludeOptions(_webSiteDeploymentProvider); } }
        public WebSiteDeploymentIncludeOptions Include { get { return new WebSiteDeploymentIncludeOptions(_webSiteDeploymentProvider); } }
    }
}