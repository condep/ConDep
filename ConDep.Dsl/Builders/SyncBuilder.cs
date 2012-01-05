using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
{
	public class SyncBuilder
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public SyncBuilder(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public SyncBuilder WithConfiguration(Action<ConfigurationBuilder> action)
		{
			var configBuilder = new ConfigurationBuilder(_webDeployDefinition.Configuration);
			action(configBuilder);
			return this;
		}

		public SyncBuilder UsingProvider(Action<ProviderCollectionBuilder> action)
		{
			var providerBuilder = new ProviderCollectionBuilder(_webDeployDefinition.Providers);
			action(providerBuilder);
			return this;
		}

		public FromBuilder From
		{
			get { return new FromBuilder(_webDeployDefinition.Source, this); }
		}

		public ToBuilder To
		{
			get { return new ToBuilder(_webDeployDefinition); }
		}
	}
}