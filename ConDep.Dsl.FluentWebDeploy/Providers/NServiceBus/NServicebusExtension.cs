using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
    public static class NServicebusExtension
    {
        public static NServiceBusBuilder NServiceBus(this ProviderCollectionBuilder providerCollectionBuilder, string path)
        {
            var nservicebusProvider = new NServiceBusProvider(path);
            providerCollectionBuilder.AddProvider(nservicebusProvider);
            return new NServiceBusBuilder(nservicebusProvider);
        }
    }
}