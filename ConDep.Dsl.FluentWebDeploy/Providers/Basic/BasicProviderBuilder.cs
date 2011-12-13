using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class BasicProviderBuilder : IProviderBuilder<BasicProviderBuilder>
	{
		private readonly BasicProvider _provider;

		public BasicProviderBuilder(BasicProvider provider)
		{
			_provider = provider;
		}

		public BasicProviderBuilder Add(string name, string value)
		{
			_provider.ProviderSettings.Add(name, value);
			return this;
		}
	}
}