using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this ProvideForDeployment providerCollection, string sourceDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions)
        {
            //var options = (DeploymentProviderOptions) providerCollection;
            var nservicebusProvider = new NServiceBusProvider(sourceDir, serviceName);
	        nServiceBusOptions(new NServiceBusOptions(nservicebusProvider));
            ((IProvideOptions)providerCollection).AddProviderAction(nservicebusProvider);
            //options.WebDeploySetup.ConfigureProvider(nservicebusProvider);
        }
    }
}