using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class RunCmdExtension
	{
        public static void RunCmd(this ProvideForInfrastructure providerOptions, string command)
		{
            RunCmd(providerOptions, command, false);		    
		}

        public static void RunCmd(this ProvideForInfrastructure providerOptions, string command, bool continueOnError)
        {
            //var options = (InfrastructureProviderOptions) providerOptions;
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
            ((IProvideOptions)providerOptions).AddProviderAction(runCmdProvider);
            //options.WebDeploySetup.ConfigureProvider(runCmdProvider);
		}

        public static void RunCmd(this ProvideForInfrastructure providerOptions, string command, bool continueOnError, Action<RunCmdOptions> runCmdOptions)
		{
            //var options = (InfrastructureProviderOptions)providerOptions;
            var runCmdProvider = new RunCmdProvider(command, continueOnError);
			runCmdOptions(new RunCmdOptions(runCmdProvider));
            ((IProvideOptions) providerOptions).AddProviderAction(runCmdProvider);
            //options.WebDeploySetup.ConfigureProvider(runCmdProvider);
        }

        public static void RunCmd(this ProvideForDeployment providerOptions, string command)
        {
            RunCmd(providerOptions, command, false);
        }

        public static void RunCmd(this ProvideForDeployment providerOptions, string command, bool continueOnError)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var runCmdProvider = new RunCmdProvider(command, continueOnError);
            ((IProvideOptions)providerOptions).AddProviderAction(runCmdProvider);
            //options.WebDeploySetup.ConfigureProvider(runCmdProvider);
        }

        public static void RunCmd(this ProvideForDeployment providerOptions, string command, bool continueOnError, Action<RunCmdOptions> runCmdOptions)
        {
            //var options = (InfrastructureProviderOptions)providerOptions;
            var runCmdProvider = new RunCmdProvider(command, continueOnError);
            runCmdOptions(new RunCmdOptions(runCmdProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(runCmdProvider);
            //options.WebDeploySetup.ConfigureProvider(runCmdProvider);
        }
    }
}