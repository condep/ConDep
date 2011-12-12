using ConDep.Dsl.FluentWebDeploy;

namespace TestWebDeployApp
{
	//ToDo: ILMerge
	public class Program : ConDepConsoleApp<Program, DeploymentSettings>
	{
		static void Main(string[] args)
		{
			Initialize(args);
		}

		protected override void Execute()
		{
			Sync(s => s
							.From.Server(Settings.FromServer)
			          	.UsingProvider(p => p.WebApp(Settings.WebAppName)
			          	                    	.AddToRemoteWebsite(Settings.RemoteWebSite)
			          	                    	.WithRemoteAppName(Settings.RemoteWebApp))
															
							.To.Server(Settings.ToServer));
		}
	}
}
