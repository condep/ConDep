using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class CopyFileExtension
	{
		public static void CopyFile(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<CopyFileBuilder> configuration)
		{
			var copyFileProvider = new CopyFileProvider(path);
			configuration(new CopyFileBuilder(copyFileProvider));
			providerCollectionBuilder.AddProvider(copyFileProvider);
		}
	}
}