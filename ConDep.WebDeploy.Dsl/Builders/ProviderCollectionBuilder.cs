using System;
using System.Collections.Generic;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class ProviderCollectionBuilder
	{
		private readonly List<Provider> _providers;

		public ProviderCollectionBuilder(List<Provider> providers)
		{
			_providers = providers;
		}

		protected internal void AddProvider(Provider provider)
		{
			_providers.Add(provider);
		}

		public void DefineCustom(string providername, string sourcepath, string destinationpath)
		{
			var provider = new BasicProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath};
			_providers.Add(provider);
		}

		public void DefineCustom(string providername, string sourcepath, string destinationpath, Action<CustomProviderOptionsBuilder> action)
		{
			var provider = new BasicProvider { Name = providername, SourcePath = sourcepath, DestinationPath = destinationpath };
			var providerOptions = new CustomProviderOptionsBuilder(provider);
			action(providerOptions);
			_providers.Add(provider);
		}
	}
}