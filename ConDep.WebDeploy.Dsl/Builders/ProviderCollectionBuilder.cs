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
	}
}