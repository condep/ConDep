using System;
using ConDep.Dsl;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this ProvideForInfrastructure providerOptions, string command)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
            //options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }

        public static void PowerShell(this ProvideForInfrastructure providerOptions, string command, Action<PowerShellOptions> powerShellOptions)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
            //options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, string command)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
            //options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, string command, Action<PowerShellOptions> powerShellOptions)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
            //options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }
    }
}