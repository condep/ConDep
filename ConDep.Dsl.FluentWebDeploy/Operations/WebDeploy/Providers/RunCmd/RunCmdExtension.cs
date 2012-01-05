using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class RunCmdExtension
	{
		public static void RunCmd(this ProviderCollectionBuilder providerCollectionBuilder, string command)
		{
			var runCmdProvider = new RunCmdProvider(command);
			providerCollectionBuilder.AddProvider(runCmdProvider);
		}

		public static void RunCmd(this ProviderCollectionBuilder providerCollectionBuilder, string command, Action<RunCmdOptions> options)
		{
			var runCmdProvider = new RunCmdProvider(command);
			options(new RunCmdOptions(runCmdProvider));
			providerCollectionBuilder.AddProvider(runCmdProvider);
		}
 
	}
}