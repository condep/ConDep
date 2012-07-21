using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteDeploymentExtension
    {
        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, Action<WebSiteDeploymentOptions> options)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            options(new WebSiteDeploymentOptions(webSiteProvider));
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, string destFilePath)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            providerCollection.AddProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForExistingIisServer providerCollection, string sourceWebsiteName, string destWebSiteName, string destFilePath, Action<WebSiteDeploymentOptions> options)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            options(new WebSiteDeploymentOptions(webSiteProvider));
            providerCollection.AddProvider(webSiteProvider);
        }
    }
}