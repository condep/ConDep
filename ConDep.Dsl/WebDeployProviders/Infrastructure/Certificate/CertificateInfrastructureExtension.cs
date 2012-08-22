using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class CertificateInfrastructureExtension
    {
        public static void CopyCertificate(this IProvideForDeployment serverOptions, string searchString, X509FindType findType)
        {
            var options = (DeploymentProviderOptions) serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, findType);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForDeployment serverOptions, string searchString, string certFriendlyName)
        {
            var options = (DeploymentProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, certFriendlyName);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForDeployment serverOptions, string certFile)
        {
            var options = (DeploymentProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(certFile);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }
    }
}