using System.Collections.Generic;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Builders
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
	}
}