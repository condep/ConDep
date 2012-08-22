using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CopyFileExtension
	{

        public static void CopyFile(this IProvideForDeployment providerOptions, string path)
        {
            var options = (DeploymentProviderOptions) providerOptions;
            var copyFileProvider = new CopyFileProvider(path);
            options.WebDeploySetup.ConfigureProvider(copyFileProvider);
        }
        
        public static void CopyFile(this IProvideForDeployment providerOptions, string path, Action<CopyFileOptions> copyFileOptions)
		{
            var options = (DeploymentProviderOptions)providerOptions;
            var copyFileProvider = new CopyFileProvider(path);
			copyFileOptions(new CopyFileOptions(copyFileProvider));
            options.WebDeploySetup.ConfigureProvider(copyFileProvider);
        }
    }
}