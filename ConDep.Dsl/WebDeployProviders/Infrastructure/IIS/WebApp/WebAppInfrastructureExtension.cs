using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.WebApp;
using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl
{
    public static class WebAppInfrastructureExtension
    {
        public static void WebApp(this ProvideForInfrastructureIis providerOptions, string webAppName, string destinationWebSiteName)
        {
            //var options = (InfrastructureIisOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, destinationWebSiteName);
            ((IProvideOptions) providerOptions).AddProviderAction(webAppProvider);
            //options.WebDeploySetup.ConfigureProvider(webAppProvider);

        }

        public static void WebApp(this ProvideForInfrastrucutreWebSite providerOptions, string webAppName)
        {
            //var options = (InfrastructureWebSiteOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerOptions.WebSiteName);
            ((IProvideOptions)providerOptions).AddProviderAction(webAppProvider);
            //options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }

        public static void WebApp(this ProvideForInfrastrucutreWebSite providerOptions, string webAppName, Action<WebAppInfrastructureOptions> webAppOptions)
        {
            //var options = (InfrastructureWebSiteOptions)providerOptions;
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerOptions.WebSiteName);
            webAppOptions(new WebAppInfrastructureOptions(webAppProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(webAppProvider);
            //options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }
    }
}