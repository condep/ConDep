using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class GeneralProviderBuilder : IProviderBuilder<GeneralProviderBuilder>
	{
		private readonly GeneralProvider _provider;

		public GeneralProviderBuilder(GeneralProvider provider)
		{
			_provider = provider;
		}

		public GeneralProviderBuilder Add(string name, string value)
		{
			_provider.ProviderSettings.Add(name, value);
			return this;
		}
	}
}