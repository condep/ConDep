using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public class WebSiteExcludeOptions : IProvideOptions<WebSiteExcludeOptions>
    {
        private readonly WebSiteProvider _webSiteProvider;

        public WebSiteExcludeOptions(WebSiteProvider webSiteProvider)
        {
            _webSiteProvider = webSiteProvider;
        }

        public WebSiteExcludeOptions AppPools()
        {
            _webSiteProvider.ExcludeAppPools = true;
            return this;
        }

        public WebSiteExcludeOptions Certificates()
        {
            _webSiteProvider.ExcludeCertificates = true;
            return this;
        }

        public WebSiteExcludeOptions Content()
        {
            _webSiteProvider.ExcludeContent = true;
            return this;
        }

        public WebSiteExcludeOptions FrameworkConfig()
        {
            _webSiteProvider.ExcludeFrameworkConfig = true;
            return this;
        }

        public WebSiteExcludeOptions CertificatesOnIisBindings()
        {
            _webSiteProvider.ExcludeHttpCertConfig = true;
            return this;
        }
    }
}