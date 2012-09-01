using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Deployment.IIS.WebSite;

namespace ConDep.Dsl
{
    public static class WebSiteDeploymentExtension
    {
        public static void WebSite(this ProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            ((IProvideOptions)providerOptions).AddProviderAction(webSiteProvider);
        }

        public static void WebSite(this ProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, Action<WebSiteDeploymentOptions> webSiteOptions)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName);
            webSiteOptions(new WebSiteDeploymentOptions(webSiteProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(webSiteProvider);
        }

        public static void WebSite(this ProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, string destFilePath)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            ((IProvideOptions)providerOptions).AddProviderAction(webSiteProvider);
        }

        public static void WebSite(this ProvideForDeploymentIis providerOptions, string sourceWebsiteName, string destWebSiteName, string destFilePath, Action<WebSiteDeploymentOptions> webSiteOptions)
        {
            var webSiteProvider = new WebSiteDeploymentProvider(sourceWebsiteName, destWebSiteName, destFilePath);
            webSiteOptions(new WebSiteDeploymentOptions(webSiteProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(webSiteProvider);
        }
    }
}