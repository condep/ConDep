using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
{
	public class ProviderCollectionBuilder
	{
		private readonly List<IProvide> _providers;

		public ProviderCollectionBuilder(List<IProvide> providers)
		{
			_providers = providers;
		}

		protected internal void AddProvider(IProvide provider)
		{
			_providers.Add(provider);

			if(provider is CompositeProvider)
			{
				((CompositeProvider) provider).Configure();
			}
		}
	}
}