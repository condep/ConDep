using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class CopyFileExtension
	{
		public static CopyFileBuilder CopyFile(this ProviderBuilder providerBuilder, string path)
		{
			var copyFileProvider = new CopyFileProvider(path);
			providerBuilder.AddProvider(copyFileProvider);
			return new CopyFileBuilder(copyFileProvider);
		}
	}
}