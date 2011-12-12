using System.Diagnostics;
using ConDep.WebDeploy.Dsl;
using ConDep.WebDeploy.Dsl.Deployment;

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

		protected override void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e)
		{
			if(e.Level == TraceLevel.Warning)
			{
				Trace.TraceWarning(e.Message);
			}
			else
			{
				Trace.TraceInformation(e.Message);
			}
		}

		protected override void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e)
		{
			Trace.TraceError(e.Message);
		}
	}
}