using System;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public interface IProvide<Implementor>
	{
		Implementor UsingProvider(Action<ProviderBuilder> action);
	}
}