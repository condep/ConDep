using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class CustomProviderOptionsBuilder
	{
		private readonly CustomProvider _provider;

		public CustomProviderOptionsBuilder(CustomProvider provider)
		{
			_provider = provider;
		}

		public CustomProviderOptionsBuilder Define(string name, string value)
		{
			_provider.ProviderSettings.Add(name, value);
			return this;
		}
	}
}