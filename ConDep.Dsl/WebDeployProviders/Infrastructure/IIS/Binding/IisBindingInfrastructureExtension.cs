using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.Binding;
using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl
{
    public static class IisBindingInfrastructureExtension
    {
        public static void HttpBinding(this ProvideForInfrastrucutreWebSite providerCollection, int port)
        {
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            ((IProvideOptions)providerCollection).AddProviderAction(httpBindingProvider);
        }

        public static void HttpBinding(this ProvideForInfrastrucutreWebSite providerCollection, int port, Action<IisBindingInfrastructureOptions> bindingOptions)
        {
            var httpBindingProvider = new HttpBindingInfrastructureProvider(providerCollection.WebSiteName, port);
            bindingOptions(new IisBindingInfrastructureOptions(httpBindingProvider));
            ((IProvideOptions)providerCollection).AddProviderAction(httpBindingProvider);
        }
        
        public static void HttpsBinding(this ProvideForInfrastrucutreWebSite providerCollection, int port, string certificateCommonName)
        {
            var httpsBindingProvider = new HttpsBindingInfrastructureProvider(port, certificateCommonName);
            ((IProvideOptions)providerCollection).AddProviderAction(httpsBindingProvider);
        }
    }
}