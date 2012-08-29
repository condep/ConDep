using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class CertificateInfrastructureExtension
    {
        public static void CopyCertificate(this ProvideForDeployment serverOptions, string searchString, X509FindType findType)
        {
            //var options = (DeploymentProviderOptions) serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, findType);
            ((IProvideOptions) serverOptions).AddProviderAction(certificateProvider);
            //options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this ProvideForDeployment serverOptions, string searchString, string certFriendlyName)
        {
            //var options = (DeploymentProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, certFriendlyName);
            ((IProvideOptions)serverOptions).AddProviderAction(certificateProvider);
            //options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this ProvideForDeployment serverOptions, string certFile)
        {
            //var options = (DeploymentProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(certFile);
            ((IProvideOptions)serverOptions).AddProviderAction(certificateProvider);
            //options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }
    }
}