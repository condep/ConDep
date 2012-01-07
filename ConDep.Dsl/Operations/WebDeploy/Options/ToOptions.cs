using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class ToOptions
	{
		private readonly WebDeployDefinition _definition;
		private readonly Destination _destination;

		public ToOptions(WebDeployDefinition definition)
		{
			_definition = definition;
			_destination = definition.Destination;
		}

		public void LocalHost()
		{
			_destination.ComputerName = "127.0.0.1";
		}

		public void LocalHost(Action<CredentialsOptions> credentials)
		{
			_destination.ComputerName = "127.0.0.1";
			var credBuilder = new CredentialsOptions(_destination.Credentials);
			credentials(credBuilder);
		}

		public void Server(string serverName)
		{
			_destination.ComputerName = serverName;
		}

		public void Server(string serverName, Action<CredentialsOptions> credentials)
		{
			_destination.ComputerName = serverName;
			var credBuilder = new CredentialsOptions(_destination.Credentials);
			credentials(credBuilder);
		}

	}
}