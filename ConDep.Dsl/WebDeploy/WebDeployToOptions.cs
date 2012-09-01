using System;

namespace ConDep.Dsl.WebDeploy
{
	public class WebDeployToOptions
	{
		private readonly WebDeployServerDefinition _serverDefinition;
		private readonly WebDeployDestination _webDeployDestination;

		public WebDeployToOptions(WebDeployServerDefinition serverDefinition)
		{
			_serverDefinition = serverDefinition;
			_webDeployDestination = serverDefinition.WebDeployDestination;
		}

		public void LocalHost()
		{
			_webDeployDestination.ComputerName = "127.0.0.1";
		}

		public void LocalHost(Action<WebDeployCredentialsOptions> credentials)
		{
			_webDeployDestination.ComputerName = "127.0.0.1";
			var credBuilder = new WebDeployCredentialsOptions(_webDeployDestination.Credentials);
			credentials(credBuilder);
		}

		public void Server(string serverName)
		{
			_webDeployDestination.ComputerName = serverName;
		}

		public void Server(string serverName, Action<WebDeployCredentialsOptions> credentials)
		{
			_webDeployDestination.ComputerName = serverName;
			var credBuilder = new WebDeployCredentialsOptions(_webDeployDestination.Credentials);
			credentials(credBuilder);
		}

	}
}