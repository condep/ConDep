using System;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public interface IOperateFrom<implementor>
	{
		implementor FromLocalHost();
		implementor FromServer(string serverName);
		implementor FromServer(string serverName, Action<CredentialsBuilder> action);
	}
}