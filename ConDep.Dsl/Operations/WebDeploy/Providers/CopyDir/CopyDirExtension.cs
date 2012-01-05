using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class CopyDirExtension
	{
		public static void CopyDir(this ProviderCollectionBuilder providerCollectionBuilder, string sourceDir, Action<CopyDirOptions> options)
		{
			var copyDirProvider = new CopyDirProvider(sourceDir);
			options(new CopyDirOptions(copyDirProvider));
			providerCollectionBuilder.AddProvider(copyDirProvider);
		}
	}
}