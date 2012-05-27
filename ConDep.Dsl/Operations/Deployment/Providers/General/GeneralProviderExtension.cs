using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class GeneralProviderExtension
	{
		public static void DefineCustom(this ProviderOptions providerOptions, string providername, string sourcepath, string destinationpath)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			providerOptions.AddProvider(provider);
		}

		public static void DefineCustom(this ProviderOptions providerOptions, string providername, string sourcepath, string destinationpath, Action<GeneralProviderOptions> options)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			var generalProviderOptions = new GeneralProviderOptions(provider);
			options(generalProviderOptions);
			providerOptions.AddProvider(provider);
		}

	}
}