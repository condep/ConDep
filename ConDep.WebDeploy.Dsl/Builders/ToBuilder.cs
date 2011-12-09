using System;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class ToBuilder
	{
		private readonly Destination _destination;

		public ToBuilder(Destination destination)
		{
			_destination = destination;
		}

		public void LocalHost()
		{
			_destination.ComputerName = "127.0.0.1";
		}

		public void LocalHost(Action<CredentialsBuilder> credentials)
		{
			_destination.ComputerName = "127.0.0.1";
			var credBuilder = new CredentialsBuilder(_destination.CredentialsProvider);
			credentials(credBuilder);
		}

		public void Server(string serverName)
		{
			_destination.ComputerName = serverName;
		}

		public void Server(string serverName, Action<CredentialsBuilder> credentials)
		{
			_destination.ComputerName = serverName;
			var credBuilder = new CredentialsBuilder(_destination.CredentialsProvider);
			credentials(credBuilder);
		}

	}
}