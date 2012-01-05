using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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