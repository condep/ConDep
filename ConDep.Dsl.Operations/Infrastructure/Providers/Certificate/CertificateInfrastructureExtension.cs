using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class CertificateInfrastructureExtension
    {
        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string searchString, X509FindType findType)
        {
            var options = (InfrastructureProviderOptions) serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, findType);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string searchString, string certFriendlyName)
        {
            var options = (InfrastructureProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(searchString, certFriendlyName);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string certFile)
        {
            var options = (InfrastructureProviderOptions)serverOptions;
            var certificateProvider = new CertificateInfrastructureProvider(certFile);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }
    }
}