using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyFile;

namespace ConDep.Dsl
{
	public static class CopyFileExtension
	{

        public static void CopyFile(this ProvideForDeployment providerOptions, string path)
        {
            var copyFileProvider = new CopyFileProvider(path);
            ((IProvideOptions)providerOptions).AddProviderAction(copyFileProvider);
        }
        
        public static void CopyFile(this ProvideForDeployment providerOptions, string path, Action<CopyFileOptions> copyFileOptions)
		{
            var copyFileProvider = new CopyFileProvider(path);
			copyFileOptions(new CopyFileOptions(copyFileProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(copyFileProvider);
        }
    }
}