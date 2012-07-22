using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class AppPoolInfrastructureExtension
    {
        public static void AppPool(this IProvideForInfrastructureIis providerCollection, string appPoolName)
        {
            var appPoolProvider = new AppPoolInfrastructureProvider(appPoolName);
            providerCollection.AddProvider(appPoolProvider);
        }

        public static void AppPool(this IProvideForInfrastructureIis providerCollection, string appPoolName, Action<AppPoolInfrastructureOptions> options)
        {
            var appPoolProvider = new AppPoolInfrastructureProvider(appPoolName);

            options(new AppPoolInfrastructureOptions(appPoolProvider));
            providerCollection.AddProvider(appPoolProvider);
        }
    }
}