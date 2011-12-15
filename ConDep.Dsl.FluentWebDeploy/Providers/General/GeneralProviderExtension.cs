using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class GeneralProviderExtension
	{
		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath)
		{
			var provider = new GeneralProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath };
			providerCollectionBuilder.AddProvider(provider);
		}

		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath, Action<GeneralProviderBuilder> configuration)
		{
			var provider = new GeneralProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath };
			var providerOptions = new GeneralProviderBuilder(provider);
			configuration(providerOptions);
			providerCollectionBuilder.AddProvider(provider);
		}

	}
}