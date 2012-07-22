using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class RunCmdExtension
	{
        public static void RunCmd(this IProviderForAll providerOptions, string command)
		{
            RunCmd(providerOptions, command, false);		    
		}

        public static void RunCmd(this IProviderForAll providerOptions, string command, bool continueOnError)
		{
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
			providerOptions.AddProvider(runCmdProvider);
		}

        public static void RunCmd(this IProviderForAll providerOptions, string command, bool continueOnError, Action<RunCmdOptions> options)
		{
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
			options(new RunCmdOptions(runCmdProvider));
			providerOptions.AddProvider(runCmdProvider);
		}
	}
}