using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class CustomProviderOptionsBuilder
	{
		private readonly BasicProvider _provider;

		public CustomProviderOptionsBuilder(BasicProvider provider)
		{
			_provider = provider;
		}

		public CustomProviderOptionsBuilder Add(string name, string value)
		{
			_provider.ProviderSettings.Add(name, value);
			return this;
		}
	}
}