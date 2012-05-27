using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this ProviderCollection providerCollection, string command)
        {
            var powerShellProvider = new PowerShellProvider(command);
            providerCollection.AddProvider(powerShellProvider);
        }

        public static void PowerShell(this IProvideForServer providerCollection, string command)
        {
            var powerShellProvider = new PowerShellProvider(command);
            providerCollection.AddProvider(powerShellProvider);
        }

        public static void PowerShell(this ProviderCollection providerCollection, string command, Action<PowerShellOptions> options)
        {
            var powerShellProvider = new PowerShellProvider(command);
            options(new PowerShellOptions(powerShellProvider));
            providerCollection.AddProvider(powerShellProvider);
        }

        public static void PowerShell(this IProvideForServer providerCollection, string command, Action<PowerShellOptions> options)
        {
            var powerShellProvider = new PowerShellProvider(command);
            options(new PowerShellOptions(powerShellProvider));
            providerCollection.AddProvider(powerShellProvider);
        }
    }
}