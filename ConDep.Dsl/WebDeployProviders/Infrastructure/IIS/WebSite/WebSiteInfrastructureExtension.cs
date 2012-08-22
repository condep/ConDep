using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteInfrastructureExtension
    {
        public static void WebSite(this IProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir)
        {
            var options = (InfrastructureIisOptions) providerCollection;
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);
            options.WebDeploySetup.ConfigureProvider(customWebSiteProvider);
        }

        public static void WebSite(this IProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir, Action<IProvideForInfrastrucutreWebSite> webSiteOptions)
        {
            var options = (InfrastructureIisOptions)providerCollection;
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);

            webSiteOptions(new InfrastructureWebSiteOptions(options.WebDeploySetup, customWebSiteProvider));
            options.WebDeploySetup.ConfigureProvider(customWebSiteProvider);
        }
    }
}