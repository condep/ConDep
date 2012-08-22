using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this IProvideForInfrastructure providerOptions, string command)
        {
            var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }

        public static void PowerShell(this IProvideForInfrastructure providerOptions, string command, Action<PowerShellOptions> powerShellOptions)
        {
            var options = (InfrastructureProviderOptions)providerOptions;
            var powerShellProvider = new PowerShellProvider(command);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            options.WebDeploySetup.ConfigureProvider(powerShellProvider);
        }
    }
}