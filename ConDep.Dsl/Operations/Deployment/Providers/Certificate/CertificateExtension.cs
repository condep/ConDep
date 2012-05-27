using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class CertificateExtension
	{
        public static void Certificate(this ProviderCollection providerCollection, string thumbprint)
		{
			var certificateProvider = new CertficiateProvider(thumbprint);
			providerCollection.AddProvider(certificateProvider);
		}

        public static void Certificate(this IProvideForServer serverOptions, string thumbprint)
        {
            var certificateProvider = new CertficiateProvider(thumbprint);
            serverOptions.AddProvider(certificateProvider);
        }
    }
}