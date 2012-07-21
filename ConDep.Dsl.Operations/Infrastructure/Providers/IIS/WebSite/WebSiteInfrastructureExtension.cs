using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteInfrastructureExtension
    {
        public static void WebSite(this IProvideForCustomIisDefinition providerCollection, string webSiteName, int id, string physicalDir)
        {
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);
            providerCollection.AddProvider(customWebSiteProvider);
        }

        public static void WebSite(this IProvideForCustomIisDefinition providerCollection, string webSiteName, int id, string physicalDir, Action<WebSiteInfrastructureOptions> options)
        {
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);

            options(new WebSiteInfrastructureOptions(customWebSiteProvider));
            providerCollection.AddProvider(customWebSiteProvider);
        }
    }
}