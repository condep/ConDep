using ConDep.WebDeploy.Dsl;

namespace TestWebDeployApp
{
	public class Program : ConDepConsoleApp<Program, DeploymentSettings>
	{
		static void Main(string[] args)
		{
			//var program = Initialize(args);
			var program = Initialize(args);

			program.Sync(s => s.FromLocalHost()
			                  	.UsingProvider(p =>
			                  	               	{
			                  	               		p.Certificate("");
			                  	               		p.DefineCustom("providerName", program.Settings.ServerName, "destinationPath");
			                  	               		p.DefineCustom("providerName", program.Settings.IP, "destinationPath", cpo =>
			                  	               		                                                                	{
			                  	               		                                                                		cpo.Add(
			                  	               		                                                                			"name",
			                  	               		                                                                			"value");
			                  	               		                                                                		cpo.Add(
			                  	               		                                                                			"name",
			                  	               		                                                                			"value");
			                  	               		                                                                	});
			                  	               	}));


		}

	}

	public class DeploymentSettings : ConDepConfiguration
	{
		public string ServerName { get; set; }
		public string IP { get; set; }
	}
}
