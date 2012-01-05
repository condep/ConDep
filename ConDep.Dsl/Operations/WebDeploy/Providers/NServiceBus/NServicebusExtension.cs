using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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