using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
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

			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.Credentials);
			action(credBuilder);
			return this;
		}
	}
}