using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class CopyFileExtension
	{
		public static void CopyFile(this ProviderCollectionBuilder providerCollectionBuilder, string path, Action<CopyFileOptions> options)
		{
			var copyFileProvider = new CopyFileProvider(path);
			options(new CopyFileOptions(copyFileProvider));
			providerCollectionBuilder.AddProvider(copyFileProvider);
		}
	}
}