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

        public static void Certificate(this IProvideForDeployment serverOptions, string thumbprint)
        {
            var certificateProvider = new CertficiateProvider(thumbprint);
            serverOptions.AddProvider(certificateProvider);
        }
    }
}