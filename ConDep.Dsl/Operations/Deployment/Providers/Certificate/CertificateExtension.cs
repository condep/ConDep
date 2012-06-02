using System.IO;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class CertificateExtension
	{
        public static void Certificate(this ProviderOptions providerOptions, string thumbprint)
		{
			var certificateProvider = new CertficiateProvider(thumbprint);
			providerOptions.AddProvider(certificateProvider);
		}

        public static void Certificate(this IProvideForDeployment serverOptions, string searchString, X509FindType findType)
        {
            var certificateProvider = new CustomCertificateProvider(searchString, findType);
            serverOptions.AddProvider(certificateProvider);
        }

        public static void Certificate(this IProvideForDeployment serverOptions, string certFile, X509ContentType contentType, StoreName storeName, StoreLocation storeLocation)
        {
            var certificateProvider = new CustomCertificateProvider(certFile, contentType, storeName, storeLocation);
            serverOptions.AddProvider(certificateProvider);
        }
    }
}