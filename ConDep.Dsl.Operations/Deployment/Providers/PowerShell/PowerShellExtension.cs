using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this IProvideForAll providerOptions, string command)
        {
            var powerShellProvider = new PowerShellProvider(command);
            providerOptions.AddProvider(powerShellProvider);
        }

        public static void PowerShell(this IProvideForAll providerOptions, string command, Action<PowerShellOptions> options)
        {
            var powerShellProvider = new PowerShellProvider(command);
            options(new PowerShellOptions(powerShellProvider));
            providerOptions.AddProvider(powerShellProvider);
        }
    }
}