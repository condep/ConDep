using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class CopyFileExtension
	{
		public static CopyFileBuilder CopyFile(this ProviderCollectionBuilder providerCollectionBuilder, string path)
		{
			var copyFileProvider = new CopyFileProvider(path);
			providerCollectionBuilder.AddProvider(copyFileProvider);
			return new CopyFileBuilder(copyFileProvider);
		}
	}
}