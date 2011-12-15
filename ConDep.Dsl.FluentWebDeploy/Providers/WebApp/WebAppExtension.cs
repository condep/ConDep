using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class WebAppExtension
	{
		public static void WebApp(this ProviderCollectionBuilder providerCollectionBuilder, string sourcePath, Action<WebAppBuilder> configuration)
		{
			var webAppProvider = new WebAppProvider(sourcePath);
			configuration(new WebAppBuilder(webAppProvider));
			providerCollectionBuilder.AddProvider(webAppProvider);
		}

	}
}