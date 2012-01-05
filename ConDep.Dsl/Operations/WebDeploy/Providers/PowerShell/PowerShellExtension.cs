using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this ProviderCollectionBuilder providerCollectionBuilder, string command)
        {
            var powerShellProvider = new PowerShellProvider(command);
            providerCollectionBuilder.AddProvider(powerShellProvider);
        }
    }
}