using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class CertificateExtension
	{
		public static void Certificate(this ProviderCollectionBuilder providerCollectionBuilder, string thumbprint)
		{
			var certificateProvider = new CertficiateProvider(thumbprint);
			providerCollectionBuilder.AddProvider(certificateProvider);
		}
	}
}