using System;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class DeleteBuilder
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public DeleteBuilder(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public DeleteBuilder UsingProvider(Action<ProviderCollectionBuilder> action)
		{
			var providerBuilder = new ProviderCollectionBuilder(_webDeployDefinition.Providers);
			action(providerBuilder);
			return this;
		}

		public DeleteBuilder FromLocalHost()
		{
			_webDeployDefinition.Source.LocalHost = true;
			return this;
		}

		public DeleteBuilder FromServer(string serverName)
		{
			_webDeployDefinition.Source.ComputerName = serverName;
			return this;
		}

		public DeleteBuilder FromServer(string serverName, Action<CredentialsBuilder> action)
		{
			_webDeployDefinition.Source.ComputerName = serverName;

			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.CredentialsProvider);
			action(credBuilder);
			return this;
		}
	}
}