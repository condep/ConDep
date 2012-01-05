using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
    public static class WebSiteExtension
    {
        public static WebSiteOptions WebSite(this ProviderCollectionBuilder providerCollectionBuilder, string sourceWebsiteName, string destWebSiteName)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName);
            providerCollectionBuilder.AddProvider(webSiteProvider);

            return new WebSiteOptions(webSiteProvider);
        }
    }
}