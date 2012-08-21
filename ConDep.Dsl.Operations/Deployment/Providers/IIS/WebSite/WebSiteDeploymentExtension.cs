using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteDeploymentExtension
    {
        public static void WebSite(this IProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName)
        {
            var options = (DeploymentIisOptions)providerOptions;
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            options.WebDeploySetup.ConfigureProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, Action<WebSiteDeploymentOptions> webSiteOptions)
        {
            var options = (DeploymentIisOptions)providerOptions;
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            webSiteOptions(new WebSiteDeploymentOptions(webSiteProvider));
            options.WebDeploySetup.ConfigureProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, string destFilePath)
        {
            var options = (DeploymentIisOptions)providerOptions;
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            options.WebDeploySetup.ConfigureProvider(webSiteProvider);
        }

        public static void WebSite(this IProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, string destFilePath, Action<WebSiteDeploymentOptions> webSiteOptions)
        {
            var options = (DeploymentIisOptions)providerOptions;
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            webSiteOptions(new WebSiteDeploymentOptions(webSiteProvider));
            options.WebDeploySetup.ConfigureProvider(webSiteProvider);
        }
    }
}