using System;
using ConDep.Dsl.Core;
using ConDep.Dsl.Providers.IIS.Binding;

namespace ConDep.Dsl
{
    public static class IisBindingInfrastructureExtension
    {
        public static void HttpBinding(this IProvideForCustomWebSite providerCollection, int port)
        {
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            providerCollection.AddProvider(httpBindingProvider);
        }


        public static void HttpBinding(this IProvideForCustomWebSite providerCollection, int port, Action<IisBindingInfrastructureOptions> bindingOptions)
        {
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            bindingOptions(new IisBindingInfrastructureOptions(httpBindingProvider));
            providerCollection.AddProvider(httpBindingProvider);
        }
        
        public static void HttpsBinding(this IProvideForCustomWebSite providerCollection, int port, string certificateCommonName)
        {
            var httpsBindingProvider = new HttpsBindingInfrastructureProvider(port, certificateCommonName);
            providerCollection.AddProvider(httpsBindingProvider);
        }

        public static void HttpsBinding(this IProvideForCustomWebSite providerCollection, int port, string certificateCommonName, Action<IisBindingInfrastructureOptions> bindingOptions)
        {
            var httpsBindingProvider = new HttpsBindingInfrastructureProvider(port, certificateCommonName);
            providerCollection.AddProvider(httpsBindingProvider);
        }

    }
}