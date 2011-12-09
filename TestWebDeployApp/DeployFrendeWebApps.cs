using ConDep.WebDeploy.Dsl;

namespace TestWebDeployApp
{
	public class DeployFrendeWebApps : WebDeployOperation
	{
		public DeployFrendeWebApps() 
		{
			Sync(s => s
			          	.From.Server("ffdevweb01")
			          	.UsingProvider(p => p.WebApp(@"agent.frende.no/STS")
			          	                    	.AddToRemoteWebsite("Default Web Site")
			          	                    	.WithRemoteAppName("STSSync"))

			          	.To.LocalHost());

		}

		public override void OnWebDeployMessage(object sender, WebDeployMessegaEventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}