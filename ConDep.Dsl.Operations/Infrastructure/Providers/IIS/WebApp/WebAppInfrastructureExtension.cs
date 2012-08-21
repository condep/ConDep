using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebAppInfrastructureExtension
    {
        public static void WebApp(this IProvideForInfrastructureIis providerOptions, string webAppName, string destinationWebSiteName)
        {
            var options = (InfrastructureIisOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, destinationWebSiteName);
            options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }

        public static void WebApp(this IProvideForInfrastrucutreWebSite providerOptions, string webAppName)
        {
            var options = (InfrastructureWebSiteOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerOptions.WebSiteName);
            options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }

        public static void WebApp(this IProvideForInfrastrucutreWebSite providerOptions, string webAppName, Action<WebAppInfrastructureOptions> webAppOptions)
        {
            var options = (InfrastructureWebSiteOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerOptions.WebSiteName);
            webAppOptions(new WebAppInfrastructureOptions(webAppProvider));
            options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }
    }
}