using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class RunCmdExtension
	{
		public static void RunCmd(this ProviderCollection providerCollection, string command)
		{
			var runCmdProvider = new RunCmdProvider(command);
			providerCollection.AddProvider(runCmdProvider);
		}

		public static void RunCmd(this ProviderCollection providerCollection, string command, Action<RunCmdOptions> options)
		{
			var runCmdProvider = new RunCmdProvider(command);
			options(new RunCmdOptions(runCmdProvider));
			providerCollection.AddProvider(runCmdProvider);
		}
 
	}
}