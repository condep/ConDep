using ConDep.WebDeploy.Dsl.Builders;

namespace ConDep.WebDeploy.Dsl
{
	public static class WebAppExtension
	{
		public static WebAppBuilder WebApp(this ProviderBuilder providerBuilder, string sourcePath)
		{
			var webAppProvider = new WebAppProvider(sourcePath);
			providerBuilder.AddProvider(webAppProvider);
			return new WebAppBuilder(webAppProvider);
		}

	}
}