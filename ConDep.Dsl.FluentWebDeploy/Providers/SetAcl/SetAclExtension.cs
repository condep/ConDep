using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class SetAclExtension
	{
		public static void SetAcl(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<SetAclBuilder> configuration)
		{
			var setAclProvider = new SetAclProvider(path);
			configuration(new SetAclBuilder(setAclProvider));
			providerCollectionBuilder.AddProvider(setAclProvider);
		}
	}
}