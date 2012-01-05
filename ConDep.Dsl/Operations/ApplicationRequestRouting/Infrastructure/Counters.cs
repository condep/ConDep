using System;
using Microsoft.Web.Administration;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class Counters
	{
		private readonly ConfigurationElement _serverConfig;
		private ConfigurationElement _countersConfig;

		public Counters(ConfigurationElement serverConfig)
		{
			_serverConfig = serverConfig;
		}

		private ConfigurationElement CountersConfig
		{
			get {
				return _countersConfig ?? (_countersConfig = _serverConfig.ChildElements["applicationRequestRouting"].ChildElements["counters"]);
			}
		}

		public State State
		{
			get
			{
				return (State)CountersConfig.GetAttributeValue("state");
			}
		}

		public long CurrentRequests
		{
			get { return (long)CountersConfig.GetAttributeValue("currentRequests"); }
		}

		public bool IsHealthy
		{
			get { return (bool)CountersConfig.GetAttributeValue("isHealthy"); }
		}

		public DateTime LastReset
		{
			get { return new DateTime((long)CountersConfig.GetAttributeValue("lastResetTickCount")); }
		}	

		public long TotalRequests
		{
			get { return (long)CountersConfig.GetAttributeValue("totalRequests"); }
		}

		public long FailedRequests
		{
			get { return (long)CountersConfig.GetAttributeValue("failedRequests"); }
		}

		public long RequestPerSecond
		{
			get { return (long)CountersConfig.GetAttributeValue("requestPerSecond"); }
		}

		public long BytesSent
		{
			get { return (long)CountersConfig.GetAttributeValue("bytesSent"); }
		}

		public long BytesReceived
		{
			get { return (long)CountersConfig.GetAttributeValue("bytesReceived"); }
		}

		public int ResponseTime
		{
			get { return (int)CountersConfig.GetAttributeValue("responseTime"); }
		}

		public bool IsServerOnline
		{
			get { return (bool) _serverConfig.GetAttributeValue("enabled"); }
		}
	}
}