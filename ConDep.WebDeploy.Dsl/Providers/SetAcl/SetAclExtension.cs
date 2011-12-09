using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class SetAclExtension
	{
		public static SetAclBuilder SetAcl(this ProviderCollectionBuilder providerCollectionBuilder, string path)
		{
			var setAclProvider = new SetAclProvider(path);
			providerCollectionBuilder.AddProvider(setAclProvider);
			return new SetAclBuilder(setAclProvider);
		}
	}
}