using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class WmiNetworkAdapter
	{
		private readonly string _computerName;

		public WmiNetworkAdapter(string computerName)
		{
			_computerName = computerName;
		}

		public IEnumerable<string> ListIp(string userName, string password)
		{
			var conOptions = new ConnectionOptions
			                 	{
			                 		Username = userName,
			                 		Password = password,
			                 		Impersonation = ImpersonationLevel.Impersonate,
			                 		EnablePrivileges = true
			                 	};

			var scope = new ManagementScope("\\\\" + _computerName + "\\root\\CIMV2", conOptions);
			scope.Connect();

			var networkConfigMngmnt = new ManagementClass(scope, new ManagementPath("Win32_NetworkAdapterConfiguration"), new ObjectGetOptions());

			var networkConfiguration = networkConfigMngmnt.GetInstances();

			var listOfIps = new List<string>();
			foreach (ManagementObject config in networkConfiguration)
			{
				if ((bool)config["ipEnabled"])
				{
					var ipaddresses = (string[])config["IPAddress"];
					listOfIps.AddRange(ipaddresses);
				}
			}
			return listOfIps.Distinct();
		}
	}
}