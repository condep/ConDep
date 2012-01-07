using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class ProviderCollection
	{
		private readonly List<IProvide> _providers;

		public ProviderCollection(List<IProvide> providers)
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