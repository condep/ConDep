namespace ConDep.Dsl.Operations.Application.Deployment.WebSite
{
    public class WebSiteDeploymentExcludeOptions
    {
        private readonly WebSiteDeploymentProvider _webSiteDeploymentProvider;

        public WebSiteDeploymentExcludeOptions(WebSiteDeploymentProvider webSiteDeploymentProvider)
        {
            _webSiteDeploymentProvider = webSiteDeploymentProvider;
        }

        public WebSiteDeploymentExcludeOptions AppPools()
        {
            _webSiteDeploymentProvider.ExcludeAppPools = true;
            return this;
        }

        public WebSiteDeploymentExcludeOptions Certificates()
        {
            _webSiteDeploymentProvider.ExcludeCertificates = true;
            return this;
        }

        public WebSiteDeploymentExcludeOptions Content()
        {
            _webSiteDeploymentProvider.ExcludeContent = true;
            return this;
        }

        public WebSiteDeploymentExcludeOptions FrameworkConfig()
        {
            _webSiteDeploymentProvider.ExcludeFrameworkConfig = true;
            return this;
        }

        public WebSiteDeploymentExcludeOptions CertificatesOnIisBindings()
        {
            _webSiteDeploymentProvider.ExcludeHttpCertConfig = true;
            return this;
        }
    }
}