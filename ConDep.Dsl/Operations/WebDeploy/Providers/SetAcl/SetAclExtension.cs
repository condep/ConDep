using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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