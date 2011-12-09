using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class CopyDirExtension
	{
		public static CopyDirBuilder CopyDir(this ProviderCollectionBuilder providerCollectionBuilder, string path)
		{
			var copyDirProvider = new CopyDirProvider(path);
			providerCollectionBuilder.AddProvider(copyDirProvider);
			return new CopyDirBuilder(copyDirProvider);
		}
	}
}