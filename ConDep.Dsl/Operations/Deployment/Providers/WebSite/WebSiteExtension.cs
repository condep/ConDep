using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public static class WebSiteExtension
    {
        public static WebSiteOptions WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName);
            providerCollection.AddProvider(webSiteProvider);

            return new WebSiteOptions(webSiteProvider);
        }

        public static WebSiteOptions WebSite(this IProvideForCustomIis providerCollection, string sourceWebsiteName, string destWebSiteName, Action<IProvideForCustomWebSite> options)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName);
            providerCollection.AddProvider(webSiteProvider);

            //throw new NotImplementedException("Not implemented CustomDefinitionWebSiteOptions");
            return new WebSiteOptions(webSiteProvider);
        }
    }

    public class CustomDefinitionWebSiteOptions
    {
    }
}