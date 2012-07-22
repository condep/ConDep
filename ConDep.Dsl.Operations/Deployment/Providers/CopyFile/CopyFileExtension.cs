using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CopyFileExtension
	{

        public static void CopyFile(this IProviderForAll providerOptions, string path)
        {
            var copyFileProvider = new CopyFileProvider(path);
            providerOptions.AddProvider(copyFileProvider);
        }
        
        public static void CopyFile(this IProviderForAll providerOptions, string path, Action<CopyFileOptions> options)
		{
			var copyFileProvider = new CopyFileProvider(path);
			options(new CopyFileOptions(copyFileProvider));
			providerOptions.AddProvider(copyFileProvider);
		}
    }
}