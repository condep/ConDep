using System;
using System.Diagnostics;
using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class WebDeploy
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public WebDeploy(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public void Deploy()
		{
			var syncOptions = new DeploymentSyncOptions();
			var sourceBaseOptions = _webDeployDefinition.Source.GetSourceBaseOptions();
			var destBaseOptions = _webDeployDefinition.Destination.GetDestinationBaseOptions();

			destBaseOptions.Trace += destBaseOptions_Trace;
			destBaseOptions.TraceLevel = TraceLevel.Verbose;

			foreach (var provider in _webDeployDefinition.Source.Providers)
			{
				var sourceDepObject = provider.GetWebDeploySourceObject(sourceBaseOptions);
				var destProviderOptions = provider.GetWebDeployDestinationProviderOptions();

				if (_webDeployDefinition.Configuration.AutoDeployAgent)
				{
					destBaseOptions.TempAgent = true;
				}

				sourceDepObject.SyncTo(destProviderOptions, destBaseOptions, syncOptions);
			}

			destBaseOptions.Trace -= destBaseOptions_Trace;
		}

		void destBaseOptions_Trace(object sender, DeploymentTraceEventArgs e)
		{
			Trace.WriteLine(e.Message);
		}

		public void Delete()
		{
			throw new NotImplementedException();
		}
	}
}