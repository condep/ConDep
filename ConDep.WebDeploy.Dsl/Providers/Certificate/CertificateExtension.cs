using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class CertificateExtension
	{
		public static void Certificate(this ProviderBuilder providerBuilder, string thumbprint)
		{
			var certificateProvider = new CertficiateProvider(thumbprint);
			providerBuilder.AddProvider(certificateProvider);
		}
	}
}