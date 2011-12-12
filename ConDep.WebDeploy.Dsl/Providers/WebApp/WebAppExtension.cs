using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
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