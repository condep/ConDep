using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class WebAppExtension
	{
		public static WebAppBuilder WebApp(this ProviderCollectionBuilder providerCollectionBuilder, string sourcePath)
		{
			var webAppProvider = new WebAppProvider(sourcePath);
			providerCollectionBuilder.AddProvider(webAppProvider);
			return new WebAppBuilder(webAppProvider);
		}

	}
}