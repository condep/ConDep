using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CopyDirExtension
	{
        public static void CopyDir(this ProvideForDeployment providerCollection, string sourceDir)
        {
            //var options = (DeploymentProviderOptions) providerCollection;
            var copyDirProvider = new CopyDirProvider(sourceDir);
            ((IProvideOptions)providerCollection).AddProviderAction(copyDirProvider);
            //options.WebDeploySetup.ConfigureProvider(copyDirProvider);
        }

        public static void CopyDir(this ProvideForDeployment providerCollection, string sourceDir, Action<CopyDirOptions> copyDirOptions)
        {
            //var options = (DeploymentProviderOptions)providerCollection;
            var copyDirProvider = new CopyDirProvider(sourceDir);
            copyDirOptions(new CopyDirOptions(copyDirProvider));
            ((IProvideOptions)providerCollection).AddProviderAction(copyDirProvider);
            //options.WebDeploySetup.ConfigureProvider(copyDirProvider);
        }
    }
}