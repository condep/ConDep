using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this ProviderCollectionBuilder providerCollectionBuilder, string sourceDir, string serviceName, Action<NServiceBusBuilder> options)
        {
            var nservicebusProvider = new NServiceBusProvider(sourceDir, serviceName);
	        options(new NServiceBusBuilder(nservicebusProvider));
			providerCollectionBuilder.AddProvider(nservicebusProvider);
        }
    }
}