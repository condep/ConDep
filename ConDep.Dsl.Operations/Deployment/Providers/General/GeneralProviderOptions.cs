using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public class GeneralProviderOptions : IProvideOptions<GeneralProviderOptions>
	{
		private readonly GeneralProvider _provider;

		public GeneralProviderOptions(GeneralProvider provider)
		{
			_provider = provider;
		}

		public GeneralProviderOptions Add(string name, string value)
		{
			_provider.ProviderSettings.Add(name, value);
			return this;
		}
	}
}