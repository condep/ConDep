using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class RunCmdExtension
	{
        public static void RunCmd(this IProvideForInfrastructure providerOptions, string command)
		{
            RunCmd(providerOptions, command, false);		    
		}

        public static void RunCmd(this IProvideForInfrastructure providerOptions, string command, bool continueOnError)
        {
            var options = (InfrastructureProviderOptions) providerOptions;
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
            options.WebDeploySetup.ConfigureProvider(runCmdProvider);
		}

        public static void RunCmd(this IProvideForInfrastructure providerOptions, string command, bool continueOnError, Action<RunCmdOptions> runCmdOptions)
		{
            var options = (InfrastructureProviderOptions)providerOptions;
            var runCmdProvider = new RunCmdProvider(command, continueOnError);
			runCmdOptions(new RunCmdOptions(runCmdProvider));
            options.WebDeploySetup.ConfigureProvider(runCmdProvider);
        }
	}
}