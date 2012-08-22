namespace ConDep.Dsl
{
    public class WebSiteDeploymentIncludeOptions 
    {
        private readonly WebSiteDeploymentProvider _webSiteDeploymentProvider;

        public WebSiteDeploymentIncludeOptions(WebSiteDeploymentProvider webSiteDeploymentProvider)
        {
            _webSiteDeploymentProvider = webSiteDeploymentProvider;
        }

        public WebSiteDeploymentIncludeOptions AppPools()
        {
            _webSiteDeploymentProvider.ExcludeAppPools = false;
            return this;
        }

        public WebSiteDeploymentIncludeOptions Certificates()
        {
            _webSiteDeploymentProvider.ExcludeCertificates = false;
            return this;
        }

        public WebSiteDeploymentIncludeOptions Content()
        {
            _webSiteDeploymentProvider.ExcludeContent = false;
            return this;
        }

        public WebSiteDeploymentIncludeOptions FrameworkConfig()
        {
            _webSiteDeploymentProvider.ExcludeFrameworkConfig = false;
            return this;
        }

        public WebSiteDeploymentIncludeOptions CertificatesOnIisBindings()
        {
            _webSiteDeploymentProvider.ExcludeHttpCertConfig = false;
            return this;
        }
    }
}