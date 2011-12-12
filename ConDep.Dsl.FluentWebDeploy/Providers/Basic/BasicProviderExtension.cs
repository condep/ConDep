using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class BasicProviderExtension
	{
		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath)
		{
			var provider = new BasicProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath };
			providerCollectionBuilder.AddProvider(provider);
		}

		public static void DefineCustom(this ProviderCollectionBuilder providerCollectionBuilder, string providername, string sourcepath, string destinationpath, Action<BasicProviderBuilder> action)
		{
			var provider = new BasicProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath };
			var providerOptions = new BasicProviderBuilder(provider);
			action(providerOptions);
			providerCollectionBuilder.AddProvider(provider);
		}

	}
}