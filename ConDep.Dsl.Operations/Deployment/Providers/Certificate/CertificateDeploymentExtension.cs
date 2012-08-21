using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class CertificateDeploymentExtension
	{
        public static void Certificate(this IProvideForDeploymentIis providerOptions, string thumbprint)
        {
            var options = (DeploymentIisOptions) providerOptions;
			var certificateProvider = new CertficiateDeploymentProvider(thumbprint);
            options.WebDeploySetup.ConfigureProvider(certificateProvider);
        }
    }
}