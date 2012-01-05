using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class SetAclExtension
	{
		public static void SetAcl(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<SetAclOptions> options)
		{
			var setAclProvider = new SetAclProvider(path);
			options(new SetAclOptions(setAclProvider));
			providerCollectionBuilder.AddProvider(setAclProvider);
		}
	}
}