using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteInfrastructureExtension
    {
        public static void WebSite(this IProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir)
        {
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);
            providerCollection.AddProvider(customWebSiteProvider);
        }

        public static void WebSite(this IProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir, Action<IProvideForInfrastrucutreWebSite> options)
        {
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);

            options(new WebSiteInfrastructureProviderOptions(customWebSiteProvider));
            providerCollection.AddProvider(customWebSiteProvider);
        }
    }
}