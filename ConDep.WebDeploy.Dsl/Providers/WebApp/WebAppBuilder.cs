namespace ConDep.WebDeploy.Dsl
{
	public class WebAppBuilder : IProviderBuilder<WebAppBuilder>
	{
		private readonly WebAppProvider _webAppProvider;

		public WebAppBuilder(WebAppProvider webAppProvider)
		{
			_webAppProvider = webAppProvider;
		}

		public WebAppBuilder AddToRemoteWebsite(string webSite)
		{
			_webAppProvider.DestinationWebSite = webSite;
			return this;
		}

		public WebAppBuilder WithRemoteAppName(string appName)
		{
			_webAppProvider.DestinationAppName = appName;
			return this;
		}
	}
}