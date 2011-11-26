using System;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class DeleteBuilder : IDelete
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public DeleteBuilder(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public IDelete UsingProvider(Action<ProviderBuilder> action)
		{
			var providerBuilder = new ProviderBuilder(_webDeployDefinition.Source.Providers);
			action(providerBuilder);
			return this;
		}

		public IDelete FromLocalHost()
		{
			_webDeployDefinition.Source.LocalHost = true;
			return this;
		}

		public IDelete FromServer(string serverName)
		{
			_webDeployDefinition.Source.ComputerName = serverName;
			return this;
		}

		public IDelete FromServer(string serverName, Action<CredentialsBuilder> action)
		{
			_webDeployDefinition.Source.ComputerName = serverName;

			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.CredentialsProvider);
			action(credBuilder);
			return this;
		}
	}
}