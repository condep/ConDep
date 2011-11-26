using System;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public interface ISync : IProvide<ISync>
	{
		ISync FromLocalHost();
		ISync FromServer(string serverName);
		ISync FromServer(string serverName, Action<CredentialsBuilder> action);
		ISync WithConfiguration(Action<ConfigurationBuilder> action);
		ISync ToLocalHost();
		ISync ToLocalHost(Action<CredentialsBuilder> action);
		ISync ToServer(string serverName);
		ISync ToServer(string serverName, Action<CredentialsBuilder> action);
	}
}