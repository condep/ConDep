using System;
using System.Collections.Generic;

namespace ConDep.WebDeploy.Configuration.Dsl
{
	public class WebDeployConfiguration
	{
		public WebDeploySource Source { get; set; }

		public WebDeployDestination Destination { get; set; }
	}

	public class WebDeployDestination
	{
		public string ComputerName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class WebDeploySource
	{
		private readonly List<Provider<dynamic>> _providers = new List<Provider<dynamic>>();
		public bool LocalHost { get; set; }

		public List<Provider<T>> Providers<T>
		{
			get {
				return _providers;
			}
		}
	}

	public abstract class Provider<T>
	{
		public abstract ProviderSettings<T> SourceSettings { get; }
		public abstract ProviderSettings<T> DestSettings { get; }
	}

	//public class ProviderDestSettings : ProviderSettings
	//{
	//}

	public class ProviderSettings<T>
	{
		public string Name { get; set; }
		public string Path { get; set; }
	}

	//public class ProviderSourceSettings : ProviderSettings
	//{
	//}
}