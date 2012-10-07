using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.AppPool;

namespace ConDep.Dsl
{
    public static class AppPoolInfrastructureExtension
    {
        public static void AppPool(this ProvideForInfrastructureIis iisOptions, string appPoolName)
        {
            //var options = (InfrastructureIisOptions) iisOptions;
            var appPoolProvider = new AppPoolInfrastructureProvider(appPoolName);
            ((IProvideOptions)iisOptions).AddProviderAction(appPoolProvider);
            //options.WebDeploySetup.ConfigureProvider(appPoolProvider);
        }

        public static void AppPool(this ProvideForInfrastructureIis iisOptions, string appPoolName, Action<AppPoolInfrastructureOptions> appPoolOptions)
        {
            //var options = (InfrastructureIisOptions)iisOptions;
            var appPoolProvider = new AppPoolInfrastructureProvider(appPoolName);

            appPoolOptions(new AppPoolInfrastructureOptions(appPoolProvider));
            ((IProvideOptions)iisOptions).AddProviderAction(appPoolProvider);
            //options.WebDeploySetup.ConfigureProvider(appPoolProvider);
        }
    }
}