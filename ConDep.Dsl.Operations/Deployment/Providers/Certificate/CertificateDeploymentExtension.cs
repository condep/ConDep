using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CertificateDeploymentExtension
	{
        public static void Certificate(this IProvideForExistingIisServer providerOptions, string thumbprint)
		{
			var certificateProvider = new CertficiateDeploymentProvider(thumbprint);
			providerOptions.AddProvider(certificateProvider);
		}
    }
}