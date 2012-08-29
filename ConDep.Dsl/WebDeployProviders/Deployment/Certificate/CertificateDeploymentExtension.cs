using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CertificateDeploymentExtension
	{
        public static void Certificate(this ProvideForDeploymentIis providerOptions, string thumbprint)
        {
			var certificateProvider = new CertficiateDeploymentProvider(thumbprint);
            ((IProvideOptions)providerOptions).AddProviderAction(certificateProvider);
        }
    }
}