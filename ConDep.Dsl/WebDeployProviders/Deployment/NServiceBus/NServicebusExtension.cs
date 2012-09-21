using System;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this ProvideForDeployment providerCollection, string sourceDir, string destDir, string serviceName)
        {
            var nservicebusProvider = new NServiceBusProvider(sourceDir, destDir, serviceName);
            ((IProvideOptions)providerCollection).AddProviderAction(nservicebusProvider);
        }

        public static void NServiceBus(this ProvideForDeployment providerCollection, string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions)
        {
            var nservicebusProvider = new NServiceBusProvider(sourceDir, destDir, serviceName);
	        nServiceBusOptions(new NServiceBusOptions(nservicebusProvider));
            ((IProvideOptions)providerCollection).AddProviderAction(nservicebusProvider);
        }
    }
}