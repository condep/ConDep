using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class WebSiteInfrastructureExtension
    {
        public static void WebSite(this ProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir)
        {
            //var options = (InfrastructureIisOptions) providerCollection;
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);
            ((IProvideOptions)providerCollection).AddProviderAction(customWebSiteProvider);
            //options.WebDeploySetup.ConfigureProvider(customWebSiteProvider);
        }

        public static void WebSite(this ProvideForInfrastructureIis providerCollection, string webSiteName, int id, string physicalDir, Action<ProvideForInfrastrucutreWebSite> webSiteOptions)
        {
            //var options = (InfrastructureIisOptions)providerCollection;
            var customWebSiteProvider = new WebSiteInfrastructureProvider(webSiteName, id, physicalDir);

            ((IProvideOptions)providerCollection).AddProviderAction(customWebSiteProvider);

            var opt = new ProvideForInfrastrucutreWebSite {WebSiteName = webSiteName};
            ((IProvideOptions)opt).AddProviderAction = customWebSiteProvider.AddChildProvider;//((IProvideOptions) providerCollection).AddProviderAction;
            webSiteOptions(opt);
            //webSiteOptions(new InfrastructureWebSiteOptions(options.WebDeploySetup, customWebSiteProvider));

            //options.WebDeploySetup.ConfigureProvider(customWebSiteProvider);
        }
    }
}