using System;
using System.IO;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.PowerShell;

namespace ConDep.Dsl
{
    public static class PowerShellExtension
    {
        public static void PowerShell(this ProvideForInfrastructure providerOptions, string commandOrScript)
        {
            var powerShellProvider = new PowerShellProvider(commandOrScript);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForInfrastructure providerOptions, string commandOrScript, Action<PowerShellOptions> powerShellOptions)
        {
            var powerShellProvider = new PowerShellProvider(commandOrScript);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForInfrastructure providerOptions, FileInfo scriptFile)
        {
            var powerShellProvider = new PowerShellProvider(scriptFile);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForInfrastructure providerOptions, FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions)
        {
            var powerShellProvider = new PowerShellProvider(scriptFile);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, string commandOrScript)
        {
            var powerShellProvider = new PowerShellProvider(commandOrScript);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, string commandOrScript, Action<PowerShellOptions> powerShellOptions)
        {
            var powerShellProvider = new PowerShellProvider(commandOrScript);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, FileInfo scriptFile)
        {
            var powerShellProvider = new PowerShellProvider(scriptFile);
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }

        public static void PowerShell(this ProvideForDeployment providerOptions, FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions)
        {
            var powerShellProvider = new PowerShellProvider(scriptFile);
            powerShellOptions(new PowerShellOptions(powerShellProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(powerShellProvider);
        }
    }
}