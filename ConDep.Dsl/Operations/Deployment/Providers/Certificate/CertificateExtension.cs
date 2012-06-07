using System.IO;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class CertificateExtension
	{
        public static void Certificate(this IProvideForExistingIisServer providerOptions, string thumbprint)
		{
			var certificateProvider = new CertficiateProvider(thumbprint);
			providerOptions.AddProvider(certificateProvider);
		}

        public static void CopyCertificate(this IProvideForDeployment serverOptions, string searchString, X509FindType findType)
        {
            var certificateProvider = new CustomCertificateProvider(searchString, findType);
            serverOptions.AddProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForDeployment serverOptions, string searchString, string certFriendlyName)
        {
            var certificateProvider = new CustomCertificateProvider(searchString, certFriendlyName);
            serverOptions.AddProvider(certificateProvider);
        }

        public static void CopyCertificate(this IProvideForDeployment serverOptions, string certFile)
        {
            var certificateProvider = new CustomCertificateProvider(certFile);
            serverOptions.AddProvider(certificateProvider);
        }
    }
}