using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class IisBindingInfrastructureExtension
    {
        public static void HttpBinding(this IProvideForInfrastrucutreWebSite providerCollection, int port)
        {
            var options = (InfrastructureWebSiteOptions) providerCollection;
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            options.WebDeploySetup.ConfigureProvider(httpBindingProvider);
        }

        public static void HttpBinding(this IProvideForInfrastrucutreWebSite providerCollection, int port, Action<IisBindingInfrastructureOptions> bindingOptions)
        {
            var options = (InfrastructureWebSiteOptions)providerCollection;
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            bindingOptions(new IisBindingInfrastructureOptions(httpBindingProvider));
            options.WebDeploySetup.ConfigureProvider(httpBindingProvider);
        }
        
        public static void HttpsBinding(this IProvideForInfrastrucutreWebSite providerCollection, int port, string certificateCommonName)
        {
            var options = (InfrastructureWebSiteOptions)providerCollection;
            var httpsBindingProvider = new HttpsBindingInfrastructureProvider(port, certificateCommonName);
            options.WebDeploySetup.ConfigureProvider(httpsBindingProvider);
        }
    }
}