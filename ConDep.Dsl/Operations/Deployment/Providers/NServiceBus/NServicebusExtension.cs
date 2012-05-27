using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this IProvideForDeployment providerCollection, string sourceDir, string serviceName, Action<NServiceBusBuilder> options)
        {
            var nservicebusProvider = new NServiceBusProvider(sourceDir, serviceName);
	        options(new NServiceBusBuilder(nservicebusProvider));
			providerCollection.AddProvider(nservicebusProvider);
        }
    }
}