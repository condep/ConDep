using ConDep.Dsl.WebDeployProviders.Deployment.CopyFile;

namespace ConDep.Dsl
{
	public static class CopyFileExtension
	{

        public static void CopyFile(this ProvideForDeployment providerOptions, string sourceFile, string destFile)
        {
            var copyFileProvider = new CopyFileProvider(sourceFile, destFile);
            ((IProvideOptions)providerOptions).AddProviderAction(copyFileProvider);
        }
    }
}