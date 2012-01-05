using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class GeneralProviderExtension
	{
		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			providerCollectionBuilder.AddProvider(provider);
		}

		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath, Action<GeneralProviderOptions> options)
		{
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			var providerOptions = new GeneralProviderOptions(provider);
			options(providerOptions);
			providerCollectionBuilder.AddProvider(provider);
		}

	}
}