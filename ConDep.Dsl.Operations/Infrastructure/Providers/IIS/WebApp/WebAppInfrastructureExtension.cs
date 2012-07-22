using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebAppInfrastructureExtension
    {
        public static void WebApp(this IProvideForInfrastructureIis providerCollection, string webAppName, string destinationWebSiteName)
        {
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, destinationWebSiteName);
            providerCollection.AddProvider(webAppProvider);
        }

        public static void WebApp(this IProvideForInfrastrucutreWebSite providerCollection, string webAppName)
        {
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerCollection.WebSiteName);
            providerCollection.AddProvider(webAppProvider);
        }

        public static void WebApp(this IProvideForInfrastrucutreWebSite providerCollection, string webAppName, Action<WebAppInfrastructureOptions> options)
        {
            var webAppProvider = new WebAppInfrastructureProvider(webAppName, providerCollection.WebSiteName);
            options(new WebAppInfrastructureOptions(webAppProvider));
            providerCollection.AddProvider(webAppProvider);
        }
    }
}