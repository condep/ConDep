using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public class WebSiteOptions : IProvideOptions<WebSiteOptions>
    {
        private readonly WebSiteProvider _webSiteProvider;

        public WebSiteOptions(WebSiteProvider webSiteProvider)
        {
            _webSiteProvider = webSiteProvider;
        }

        public WebSiteExcludeOptions Exclude
        {
            get { return new WebSiteExcludeOptions(_webSiteProvider); }
        }
    }
}