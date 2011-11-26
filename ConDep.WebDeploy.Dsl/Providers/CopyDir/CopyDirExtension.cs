using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class CopyDirExtension
	{
		public static CopyDirBuilder CopyDir(this ProviderBuilder providerBuilder, string path)
		{
			var copyDirProvider = new CopyDirProvider(path);
			providerBuilder.AddProvider(copyDirProvider);
			return new CopyDirBuilder(copyDirProvider);
		}
	}
}