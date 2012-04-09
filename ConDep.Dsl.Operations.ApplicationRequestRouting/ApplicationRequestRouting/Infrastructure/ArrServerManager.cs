using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class ArrServerManager : IDisposable
	{
		private readonly string _farm;
		private readonly string _serverIP;
		private readonly ServerManager _serverManager;
		private readonly StateExecutor _stateExecutor;
		private bool _disposed;

		public ArrServerManager(string arrServer)
		{
			_serverManager = ServerManager.OpenRemote(arrServer);
		}

		public ArrServerManager(string arrServer, string farm, string serverIp)
		{
			_farm = farm;
			_serverIP = serverIp;

			_serverManager = ServerManager.OpenRemote(arrServer);
			//_stateExecutor = new StateExecutor(GetSetStateMethodInstance(farm, serverIP));
			_stateExecutor = new StateExecutor(GetServer(farm, serverIp));
		}

		internal StateExecutor StateExecutor
		{
			get { return _stateExecutor; }
		}

		internal Counters Counters
		{
			get { return new Counters(GetServer(_farm, _serverIP)); }
		}

		public Counters GetCounters(string farm, string serverIP)
		{
			return new Counters(GetServer(farm, serverIP));
		}

		public Counters GetCounters(ConfigurationElement server)
		{
			return new Counters(server);
		}

		private ConfigurationMethodInstance GetSetStateMethodInstance(string farm, string serverIP)
		{
			return GetApplicationRequestRoutingConfig(farm, serverIP).Methods["SetState"].CreateInstance();
		}

		private ConfigurationElement GetCountersConfig(string farm, string serverIP)
		{
			return GetApplicationRequestRoutingConfig(farm, serverIP).ChildElements["counters"];
		}

		private ConfigurationElement GetApplicationRequestRoutingConfig(string farm, string serverIP)
		{
			Configuration config = _serverManager.GetApplicationHostConfiguration();
			ConfigurationSection webFarmsSection = config.GetSection("webFarms");

			foreach (var webFarm in webFarmsSection.GetCollection())
			{
				if (webFarm["name"].ToString() == farm)
				{
					foreach (var webServer in webFarm.GetCollection())
					{
						if (webServer["address"].ToString() == serverIP)
						{
							return webServer.ChildElements["applicationRequestRouting"];
						}
					}
					throw new Exception(string.Format("Server address {0} not found.", serverIP));
				}
			}
			throw new Exception(string.Format("Server farm {0} not found.", farm));
		}

		public IEnumerable<WebServer> GetFarms(string dnsServer)
		{
			Configuration config = _serverManager.GetApplicationHostConfiguration();
			ConfigurationSection webFarmsSection = config.GetSection("webFarms");

			var webServers = new List<WebServer>();
			foreach (var webFarm in webFarmsSection.GetCollection())
			{
				//var webFarmName = webFarm["name"].ToString();
				foreach (var webServerIp in webFarm.GetCollection())
				{
					var ip = webServerIp["address"].ToString();
					var webServerName = ResolveIpToServerName(dnsServer, ip);
					var webServer = webServers.FirstOrDefault(x => x.Name == webServerName);

					if(webServer == null)
					{
						webServer = new WebServer { Name = webServerName };
						webServers.Add(webServer);
					}

					var counters = GetCounters(webServerIp);
					webServer.Sites.Add(new SiteInfo
												{
													//FarmName = webFarmName,
													//Ip = ip,
													//Availability = counters.State.ToString(),
													//Healthy = counters.IsHealthy,
													Online = counters.IsServerOnline,
													//RequestsPerSecond = counters.RequestPerSecond,
													//ResponseTime = counters.ResponseTime,
													//CurrentRequests = counters.CurrentRequests,
													//TotalRequests = counters.TotalRequests,
													//FailedRequests = counters.FailedRequests,
													//BytesSent = counters.BytesSent,
													//BytesReceived = counters.BytesReceived
												});
				
				}
			}
			return webServers;
		}

		private string ResolveIpToServerName(string dnsServer, string ip)
		{
			//try
			//{
			//   var host = Dns.GetHostEntry(IPAddress.Parse(ip));
			//   return host.HostName;
			//}
			//catch
			//{
			//   return "Unresolved";
			//}
			return "Unresolved";
		}

		private ConfigurationElement GetServer(string farm, string serverIP)
		{
			Configuration config = _serverManager.GetApplicationHostConfiguration();
			ConfigurationSection webFarmsSection = config.GetSection("webFarms");

			foreach (var webFarm in webFarmsSection.GetCollection())
			{
				if (webFarm["name"].ToString() == farm)
				{
					foreach (var webServer in webFarm.GetCollection())
					{
						if (webServer["address"].ToString() == serverIP)
						{
							return webServer;
						}
					}
					throw new Exception(string.Format("Server address {0} not found.", serverIP));
				}
			}
			throw new Exception(string.Format("Server farm {0} not found.", farm));
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_serverManager.Dispose();
				}
				_disposed = true;
			}
		}

		public void Commit()
		{
			_serverManager.CommitChanges();
		}
	}

	public class WebServer
	{
		private readonly List<SiteInfo> _sites = new List<SiteInfo>();

		public string Name { get; set; }

		public List<SiteInfo> Sites { get { return _sites; } }
	}

	public class SiteInfo
	{
		public string FarmName { get; set; }

		public string Ip { get; set; }

		public string Availability { get; set; }

		public bool Online { get; set; }

		public bool Healthy { get; set; }

		public long RequestsPerSecond { get; set; }

		public int ResponseTime { get; set; }

		public long BytesReceived { get; set; }

		public long BytesSent { get; set; }

		public long FailedRequests { get; set; }

		public long TotalRequests { get; set; }

		public long CurrentRequests { get; set; }
	}

}