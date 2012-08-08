using System;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Web.Administration;

namespace ConDep.PowerShell.ApplicationRequestRouting.Infrastructure
{
	public class ArrServerManager : IDisposable
	{
		private readonly string _farm;
		private readonly string _serverName;
		private readonly ServerManager _serverManager;
		private StateExecutor _stateExecutor;
		private bool _disposed;

		public ArrServerManager()
		{
			_serverManager = new ServerManager();
		}

		public ArrServerManager(string farm, string serverName)
		{
			_farm = farm;
			_serverName = serverName;

			_serverManager = new ServerManager();
		}

		internal StateExecutor StateExecutor
		{
            get { return _stateExecutor ?? new StateExecutor(GetServer(_farm, _serverName)); }
		}

		internal Counters Counters
		{
			get { return new Counters(GetServer(_farm, _serverName)); }
		}

		public Counters GetCounters(string farm, string serverIP)
		{
			return new Counters(GetServer(farm, serverIP));
		}

		public Counters GetCounters(ConfigurationElement server)
		{
			return new Counters(server);
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
}