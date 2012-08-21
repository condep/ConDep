using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class WebAppDeploymentExtension
	{
		public static void WebApp(this IProvideForDeploymentIis providerOptions, string sourceDir, string webAppName, string destinationWebSiteName)
		{
            var options = (DeploymentIisOptions)providerOptions;
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, destinationWebSiteName);
            options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }

        public static void WebApp(this IProvideForDeploymentIis providerOptions, string sourceWebSiteName, string sourceWebAppName, string destinationWebSiteName, string destinationWebAppName)
        {
            var options = (DeploymentIisOptions)providerOptions;
            var webAppProvider = new WebAppDeploymentProvider(sourceWebSiteName, sourceWebAppName, destinationWebSiteName, destinationWebAppName);
            options.WebDeploySetup.ConfigureProvider(webAppProvider);
        }
    }
}