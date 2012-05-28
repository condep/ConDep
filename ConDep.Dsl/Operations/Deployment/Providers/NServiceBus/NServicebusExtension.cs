using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public static class NServicebusExtension
    {
        public static void NServiceBus(this IProvideForDeployment providerCollection, string sourceDir, string serviceName, Action<NServiceBusOptions> options)
        {
            var nservicebusProvider = new NServiceBusProvider(sourceDir, serviceName);
	        options(new NServiceBusOptions(nservicebusProvider));
			providerCollection.AddProvider(nservicebusProvider);
        }
    }
}