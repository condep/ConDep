using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class WebAppExtension
	{
		public static void WebApp(this IProvideForExistingIisServer providerCollection, string sourceDir, string webAppName, string destinationWebSiteName)
		{
		    AddProvider(sourceDir, webAppName, destinationWebSiteName, providerCollection);
		}

        public static void WebApp(this IProvideForExistingIisServer providerCollection, string sourceWebSiteName, string sourceWebAppName, string destinationWebSiteName, string destinationWebAppName)
        {
            var webAppProvider = new WebAppProvider(sourceWebSiteName, sourceWebAppName, destinationWebSiteName, destinationWebAppName);
            providerCollection.AddProvider(webAppProvider);
        }

	    public static void WebApp(this IProvideForCustomIisDefinition providerCollection, string sourceDir, string webAppName, string destinationWebSiteName)
        {
            AddProvider(sourceDir, webAppName, destinationWebSiteName, providerCollection);
        }

        public static void WebApp(this IProvideForCustomWebSite providerCollection, string webAppName)
        {
            var webAppProvider = new CustomWebAppProvider(webAppName, providerCollection.WebSiteName);
            providerCollection.AddProvider(webAppProvider);
        }

        private static void AddProvider(string sourceDir, string webAppName, string destinationWebSiteName, IProviderCollection providerCollection)
        {
            var webAppProvider = new WebAppProvider(sourceDir, webAppName, destinationWebSiteName);
            providerCollection.AddProvider(webAppProvider);
        }
    }
}