using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public static class WebSiteExtension
    {
        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName);
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, Action<WebSiteOptions> options)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName);
            options(new WebSiteOptions(webSiteProvider));
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, string destFilePath)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, string destFilePath, Action<WebSiteOptions> options)
        {
            var webSiteProvider = new WebSiteProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            options(new WebSiteOptions(webSiteProvider));
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForCustomIisDefinition providerCollection, string webSiteName, int id)
        {
            var customWebSiteProvider = new CustomWebSiteProvider(webSiteName, id);
            providerCollection.AddProvider(customWebSiteProvider);
        }

        public static void WebSite(this IProvideForCustomIisDefinition providerCollection, string webSiteName, int id, Action<CustomWebSiteOptions> options)
        {
            var customWebSiteProvider = new CustomWebSiteProvider(webSiteName, id);
            options(new CustomWebSiteOptions(customWebSiteProvider));
            providerCollection.AddProvider(customWebSiteProvider);
        }
    }
}