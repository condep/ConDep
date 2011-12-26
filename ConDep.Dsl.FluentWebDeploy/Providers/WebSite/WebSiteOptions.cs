using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
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