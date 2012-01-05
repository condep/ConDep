using ConDep.Dsl.Builders;

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