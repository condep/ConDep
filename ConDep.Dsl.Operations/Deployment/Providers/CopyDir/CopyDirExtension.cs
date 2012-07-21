using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CopyDirExtension
	{
        public static void CopyDir(this ProviderOptions providerOptions, string sourceDir, Action<CopyDirOptions> options)
		{
			var copyDirProvider = new CopyDirProvider(sourceDir);
			options(new CopyDirOptions(copyDirProvider));
			providerOptions.AddProvider(copyDirProvider);
		}

        public static void CopyDir(this IProvideForDeployment providerCollection, string sourceDir, Action<CopyDirOptions> options)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir);
            options(new CopyDirOptions(copyDirProvider));
            providerCollection.AddProvider(copyDirProvider);
        }
    }
}