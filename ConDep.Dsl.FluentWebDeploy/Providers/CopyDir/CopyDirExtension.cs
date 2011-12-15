using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class CopyDirExtension
	{
		public static void CopyDir(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<CopyDirBuilder> configuration)
		{
			var copyDirProvider = new CopyDirProvider(path);
			configuration(new CopyDirBuilder(copyDirProvider));
			providerCollectionBuilder.AddProvider(copyDirProvider);
		}
	}
}