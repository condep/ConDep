using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
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