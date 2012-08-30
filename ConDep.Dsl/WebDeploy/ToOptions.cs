using System;
using ConDep.Dsl;

namespace ConDep.Dsl.Operations.Deployment.Options
{
	public class ToOptions
	{
		private readonly WebDeployServerDefinition _serverDefinition;
		private readonly WebDeployDestination _webDeployDestination;

		public ToOptions(WebDeployServerDefinition serverDefinition)
		{
			_serverDefinition = serverDefinition;
			_webDeployDestination = serverDefinition.WebDeployDestination;
		}

		public void LocalHost()
		{
			_webDeployDestination.ComputerName = "127.0.0.1";
		}

		public void LocalHost(Action<CredentialsOptions> credentials)
		{
			_webDeployDestination.ComputerName = "127.0.0.1";
			var credBuilder = new CredentialsOptions(_webDeployDestination.Credentials);
			credentials(credBuilder);
		}

		public void Server(string serverName)
		{
			_webDeployDestination.ComputerName = serverName;
		}

		public void Server(string serverName, Action<CredentialsOptions> credentials)
		{
			_webDeployDestination.ComputerName = serverName;
			var credBuilder = new CredentialsOptions(_webDeployDestination.Credentials);
			credentials(credBuilder);
		}

	}
}