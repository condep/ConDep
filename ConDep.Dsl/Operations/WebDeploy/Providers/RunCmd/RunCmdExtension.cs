using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class RunCmdExtension
	{
		public static void RunCmd(this ProviderCollection providerCollection, string command)
		{
            RunCmd(providerCollection,command, false);		    
		}

        public static void RunCmd(this ProviderCollection providerCollection, string command, bool continueOnError)
		{
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
			providerCollection.AddProvider(runCmdProvider);
		}

		public static void RunCmd(this ProviderCollection providerCollection, string command, bool continueOnError, Action<RunCmdOptions> options)
		{
			var runCmdProvider = new RunCmdProvider(command, continueOnError);
			options(new RunCmdOptions(runCmdProvider));
			providerCollection.AddProvider(runCmdProvider);
		}
 
	}
}