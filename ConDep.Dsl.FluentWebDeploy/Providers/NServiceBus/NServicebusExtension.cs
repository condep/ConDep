using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<NServiceBusBuilder> configuration)
        {
            var nservicebusProvider = new NServiceBusProvider(path);
	        	configuration(new NServiceBusBuilder(nservicebusProvider));
				providerCollectionBuilder.AddProvider(nservicebusProvider);
        }
    }
}