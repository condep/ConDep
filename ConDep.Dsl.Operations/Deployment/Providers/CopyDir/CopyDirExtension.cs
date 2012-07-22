using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CopyDirExtension
	{
        public static void CopyDir(this IProviderForAll providerCollection, string sourceDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir);
            providerCollection.AddProvider(copyDirProvider);
        }

        public static void CopyDir(this IProviderForAll providerCollection, string sourceDir, Action<CopyDirOptions> options)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir);
            options(new CopyDirOptions(copyDirProvider));
            providerCollection.AddProvider(copyDirProvider);
        }

    }
}