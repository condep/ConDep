using System;
using ConDep.Dsl.Operations.WebDeploy.Options;
using ConDep.Dsl.Providers.IIS.Binding;

namespace ConDep.Dsl
{
    public static class IisBindingExtension
    {
        public static void HttpBinding(this IProvideForCustomWebSite providerCollection, int port)
        {
            var httpBindingProvider = new HttpBindingProvider(port);
            providerCollection.AddProvider(httpBindingProvider);
        }


        public static void HttpBinding(this IProvideForCustomWebSite providerCollection, int port, Action<BindingOptions> bindingOptions)
        {
            var httpBindingProvider = new HttpBindingProvider(port);
            bindingOptions(new BindingOptions(httpBindingProvider));
            providerCollection.AddProvider(httpBindingProvider);
        }
        
        public static void HttpsBinding(this IProvideForCustomWebSite providerCollection, int port, string certificateCommonName)
        {
            var httpsBindingProvider = new HttpsBindingProvider(port, certificateCommonName);
            providerCollection.AddProvider(httpsBindingProvider);
        }

        public static void HttpsBinding(this IProvideForCustomWebSite providerCollection, int port, string certificateCommonName, Action<BindingOptions> bindingOptions)
        {
            var httpsBindingProvider = new HttpsBindingProvider(port, certificateCommonName);
            providerCollection.AddProvider(httpsBindingProvider);
        }

    }
}