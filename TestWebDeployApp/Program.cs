using ConDep.WebDeploy.Dsl;

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
							.From.LocalHost()
			          	.UsingProvider(p => p.WebApp(Settings.WebAppName)
			          	                    	.AddToRemoteWebsite(Settings.RemoteWebSite)
			          	                    	.WithRemoteAppName(Settings.RemoteWebApp))
															
							.To.Server(Settings.ToServer));
		}
	}
}
