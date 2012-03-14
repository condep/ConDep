using System.Collections.Generic;
using ConDep.Dsl;
using ConDep.Dsl.Console;

namespace TestWebDeployApp
{
	public class DeploymentSettings : ConDepConfiguration
	{
		[CmdParam(true)] 
		public string ToServer = "someServer";

		[CmdParam]
		public string RemoteWebApp = "SomeRemoteWebApp";

		public string WebAppName = "SomeWebApp";
		public string RemoteWebSite = "Default Web Site";
		public string FromServer = "someServer";
	}

	public class DevEnvironment : ConDepConfigEnvironment<DeploymentSettings>
	{
		public override string Name
		{
			get { return "Dev"; }
		}

		public override IList<string> Servers
		{
			get
			{
				return new List<string>
					{
						"ffdevweb01",
						"ffdevweb02"
					};
			}
		}

		public override void InitializeSettings(DeploymentSettings settings)
		{
			throw new System.NotImplementedException();
		}
	}

}