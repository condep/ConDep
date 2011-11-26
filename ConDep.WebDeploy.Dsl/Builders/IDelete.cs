using System;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public interface IDelete : IOperation, IProvide<IDelete>
	{
		IDelete FromLocalHost();
		IDelete FromServer(string serverName);
		IDelete FromServer(string serverName, Action<CredentialsBuilder> action);
	}
}