using System;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class SyncBuilder : ISync
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public SyncBuilder(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public ISync FromLocalHost()
		{
			_webDeployDefinition.Source.LocalHost = true;
			return this;
		}

		public ISync FromServer(string serverName)
		{
			_webDeployDefinition.Source.ComputerName = serverName;
			return this;
		}

		public ISync FromServer(string serverName, Action<CredentialsBuilder> action)
		{
			_webDeployDefinition.Source.ComputerName = serverName;

			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.CredentialsProvider);
			action(credBuilder);
			return this;
		}

		public ISync WithConfiguration(Action<ConfigurationBuilder> action)
		{
			var configBuilder = new ConfigurationBuilder(_webDeployDefinition.Configuration);
			action(configBuilder);
			return this;
		}

		public ISync UsingProvider(Action<ProviderBuilder> action)
		{
			var providerBuilder = new ProviderBuilder(_webDeployDefinition.Source.Providers);
			action(providerBuilder);
			return this;
		}

		public ISync ToLocalHost()
		{
			_webDeployDefinition.Destination.ComputerName = "127.0.0.1";
			return this;
		}

		public ISync ToLocalHost(Action<CredentialsBuilder> action)
		{
			_webDeployDefinition.Destination.ComputerName = "127.0.0.1";
			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.CredentialsProvider);
			action(credBuilder);
			return this;
		}

		public ISync ToServer(string serverName)
		{
			_webDeployDefinition.Destination.ComputerName = serverName;
			return this;
		}

		public ISync ToServer(string serverName, Action<CredentialsBuilder> action)
		{
			_webDeployDefinition.Destination.ComputerName = serverName;
			var credBuilder = new CredentialsBuilder(_webDeployDefinition.Source.CredentialsProvider);
			action(credBuilder);
			return this;
		}
	}
}