using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class SyncOptions
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public SyncOptions(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public SyncOptions WithConfiguration(Action<ConfigurationOptions> action)
		{
			var configBuilder = new ConfigurationOptions(_webDeployDefinition.Configuration);
			action(configBuilder);
			return this;
		}

		public SyncOptions UsingProvider(Action<ProviderCollection> action)
		{
			var providerBuilder = new ProviderCollection(_webDeployDefinition.Providers);
			action(providerBuilder);
			return this;
		}

		public FromOptions From
		{
			get { return new FromOptions(_webDeployDefinition.Source, this); }
		}

		public ToOptions To
		{
			get { return new ToOptions(_webDeployDefinition); }
		}
	}
}