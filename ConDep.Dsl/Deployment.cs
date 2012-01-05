using System;
using System.Collections.Generic;

namespace ConDep.WebDeploy.Configuration.Dsl
{
	public class Deployment
	{
		private WebDeployConfiguration _configuration;

		public SyncOptions Sync()
		{
			_configuration = new WebDeployConfiguration();
			return new SyncTask(_configuration);
		}

		public WebDeployConfiguration Configuration
		{
			get { return _configuration; }
		}
	}

	public class SyncTask : SyncOptions
	{
		private readonly WebDeployConfiguration _configuration;

		public SyncTask(WebDeployConfiguration configuration)
		{
			_configuration = configuration;
		}

		public SourceType FromLocalHost()
		{
			_configuration.Source.LocalHost = true;
			return new SourceTypeTask(_configuration);
		}
	}

	public class SourceTypeTask : SourceType
	{
		private readonly WebDeployConfiguration _configuration;

		public SourceTypeTask(WebDeployConfiguration configuration)
		{
			_configuration = configuration;
		}

		public SourceType UsingProvider(Action<ProviderOptions> action)
		{
			action(new ProviderTask(_configuration));
			return this;
		}

		public ServerCredentialOptions ToServer(string serverName)
		{
			_configuration.Destination.ComputerName = serverName;
			return new ServerCredentialTask(_configuration);
		}
	}

	public class ProviderTask : ProviderOptions
	{
		private readonly WebDeployConfiguration _configuration;

		public ProviderTask(WebDeployConfiguration configuration)
		{
			_configuration = configuration;
		}

		public WebAppOptions WebApp(string filePath)
		{
			var provider = new WebAppProvider();
			provider.SourceSettings.Name = Microsoft.Web.Deployment.DeploymentWellKnownProvider.IisApp.ToString();
			provider.SourceSettings.Path = filePath;

			_configuration.Source.Providers.Add(provider);
			return new WebAppTask(provider);
		}
	}

	public class WebAppProvider : Provider<WebAppProvider>
	{
		private readonly ProviderSettings<WebAppProvider> _sourceSettings = new ProviderSettings<WebAppProvider>();
		private readonly ProviderSettings<WebAppProvider> _destSettings = new ProviderSettings<WebAppProvider>();

		public string RemoteAppName { get; set; }

		public override ProviderSettings<WebAppProvider> SourceSettings
		{
			get { throw new NotImplementedException(); }
		}

		public override ProviderSettings<WebAppProvider> DestSettings
		{
			get { throw new NotImplementedException(); }
		}
	}

	public class WebAppTask : WebAppOptions
	{
		private readonly Provider _provider;

		public WebAppTask(WebAppProvider provider)
		{
			_provider = provider;
		}

		public WebAppOptions SetRemoteAppNameTo(string appName)
		{
			_provider.DestSettings.RemoteAppName = appName;
		}

		public WebAppOptions SetRemotePathTo(string path)
		{
			_provider.DestSettings.Path = path;
		}

		public WebAppOptions AddToRemoteWebsite(string webSiteName)
		{
			_provider.DestSettings.
		}
	}

	public class ServerCredentialTask : ServerCredentialOptions
	{
		private readonly WebDeployConfiguration _configuration;

		public ServerCredentialTask(WebDeployConfiguration configuration)
		{
			_configuration = configuration;
		}

		public ServerCredentialOptions WithUserName(string username)
		{
			_configuration.Destination.UserName = username;
			return this;
		}

		public ServerCredentialOptions WithPassword(string password)
		{
			_configuration.Destination.Password = password;
			return this;
		}
	}

	public interface SyncOptions
	{
		SourceType FromLocalHost();
	}

	public interface SourceType
	{
		SourceType UsingProvider(Action<ProviderOptions> action);
		ServerCredentialOptions ToServer(string serverName);
	}

	public interface ServerCredentialOptions
	{
		ServerCredentialOptions WithUserName(string username);
		ServerCredentialOptions WithPassword(string password);
	}

	public interface ProviderOptions
	{
		WebAppOptions WebApp(string filePath);
	}

	public interface WebAppOptions
	{
		WebAppOptions SetRemoteAppNameTo(string appName);
		WebAppOptions SetRemotePathTo(string path);
		WebAppOptions AddToRemoteWebsite(string webSiteName);
	}
}