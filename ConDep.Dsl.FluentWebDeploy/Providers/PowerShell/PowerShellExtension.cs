using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this ProviderCollectionBuilder providerCollectionBuilder, string command)
        {
            var powerShellProvider = new PowerShellProvider(command);
            providerCollectionBuilder.AddProvider(powerShellProvider);
        }

        public static void PowerShell(this ProviderCollectionBuilder providerCollectionBuilder, string command, Action<PowerShellBuilder> configuration)
        {
            var powerShellProvider = new PowerShellProvider(command);
	        configuration(new PowerShellBuilder(powerShellProvider));
			providerCollectionBuilder.AddProvider(powerShellProvider);
        }
    }
}