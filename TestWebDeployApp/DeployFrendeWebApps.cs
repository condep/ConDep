using ConDep.WebDeploy.Dsl;

namespace TestWebDeployApp
{
	public class DeployFrendeWebApps : WebDeployOperation
	{
		public DeployFrendeWebApps()
		{
			Sync(s => s
			          	.FromServer("ffdevweb01")
			          	.UsingProvider(p => p.WebApp(@"agent.frende.no/STS")
			          	                    	.AddToRemoteWebsite("Default Web Site")
			          	                    	.SetRemoteAppNameTo("STSSync"))

			          	.ToLocalHost());

		}
	}
}