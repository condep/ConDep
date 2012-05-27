using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

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