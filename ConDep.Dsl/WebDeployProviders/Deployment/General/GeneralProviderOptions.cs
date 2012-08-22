namespace ConDep.Dsl
{
	public class GeneralProviderOptions 
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