using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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