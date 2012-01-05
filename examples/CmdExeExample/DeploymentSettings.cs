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
}