using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public static class CertificateInfrastructureExtension
    {
        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string searchString, X509FindType findType)
        {
            var certificateProvider = new CertificateInfrastructureProvider(searchString, findType);
            serverOptions.AddProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string searchString, string certFriendlyName)
        {
            var certificateProvider = new CertificateInfrastructureProvider(searchString, certFriendlyName);
            serverOptions.AddProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForInfrastructure serverOptions, string certFile)
        {
            var certificateProvider = new CertificateInfrastructureProvider(certFile);
            serverOptions.AddProvider(certificateProvider);
        }
    }
}