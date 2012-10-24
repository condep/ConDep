using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeploy;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyDir;

namespace ConDep.Dsl
{
	public static class CopyDirExtension
	{
        public static void CopyDir(this ProvideForDeployment providerCollection, string sourceDir, string destDir)
        {
            //var options = (DeploymentProviderOptions) providerCollection;
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            ((IProvideOptions)providerCollection).AddProviderAction(copyDirProvider);
            //options.WebDeploySetup.ConfigureProvider(copyDirProvider);
        }
    }
}