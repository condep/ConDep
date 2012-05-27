using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class GeneralProviderExtension
	{
		public static void DefineCustom(this ProviderCollection providerCollection, string providername, string sourcepath, string destinationpath)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			providerCollection.AddProvider(provider);
		}

		public static void DefineCustom(this ProviderCollection providerCollection, string providername, string sourcepath, string destinationpath, Action<GeneralProviderOptions> options)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			var providerOptions = new GeneralProviderOptions(provider);
			options(providerOptions);
			providerCollection.AddProvider(provider);
		}

	}
}