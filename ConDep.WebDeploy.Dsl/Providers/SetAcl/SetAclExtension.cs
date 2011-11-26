using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class SetAclExtension
	{
		public static SetAclBuilder SetAcl(this ProviderBuilder providerBuilder, string path)
		{
			var setAclProvider = new SetAclProvider(path);
			providerBuilder.AddProvider(setAclProvider);
			return new SetAclBuilder(setAclProvider);
		}
	}
}