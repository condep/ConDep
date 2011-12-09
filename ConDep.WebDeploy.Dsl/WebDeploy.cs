using System;
using System.Diagnostics;
using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class WebDeploy : IWebDeploy
	{
		public event EventHandler<WebDeployMessegaEventArgs> Output;
		public event EventHandler<WebDeployMessegaEventArgs> OutputError;

		public void Deploy(WebDeployDefinition webDeployDefinition)
		{
			try
			{
				var syncOptions = new DeploymentSyncOptions();
				var sourceBaseOptions = webDeployDefinition.Source.GetSourceBaseOptions();
				var destBaseOptions = webDeployDefinition.Destination.GetDestinationBaseOptions();

				destBaseOptions.Trace += OnWebDeployTraceMessage;
				destBaseOptions.TraceLevel = TraceLevel.Verbose;

				foreach (var provider in webDeployDefinition.Source.Providers)
				{
					var sourceDepObject = provider.GetWebDeploySourceObject(sourceBaseOptions);
					var destProviderOptions = provider.GetWebDeployDestinationProviderOptions();

					if (webDeployDefinition.Configuration.AutoDeployAgent)
					{
						destBaseOptions.TempAgent = true;
					}

					sourceDepObject.SyncTo(destProviderOptions, destBaseOptions, syncOptions);
				}

				destBaseOptions.Trace -= OnWebDeployTraceMessage;
			}
			catch(Exception ex)
			{
				if(OutputError != null)
				{
					var message = GetCompleteExceptionMessage(ex);

					OutputError(this, new WebDeployMessegaEventArgs { Message = message, Level = TraceLevel.Error });
				}
			}
		}

		private string GetCompleteExceptionMessage(Exception exception)
		{
			var message = exception.Message;
			if (exception.InnerException != null)
			{
				message += " " + GetCompleteExceptionMessage(exception.InnerException);
			}
			return message;
		}

		void OnWebDeployTraceMessage(object sender, DeploymentTraceEventArgs e)
		{
			if(e.EventLevel == TraceLevel.Error)
			{
				if(OutputError != null)
				{
					OutputError(this, new WebDeployMessegaEventArgs {Message = e.Message, Level = e.EventLevel});
				}
			}
			else
			{
				if (Output != null)
				{
					Output(this, new WebDeployMessegaEventArgs { Message = e.Message, Level = e.EventLevel });
				}
			}
		}

		public void Delete(WebDeployDefinition webDeployDefinition)
		{
			throw new NotImplementedException();
		}

		//public WebDeployDefinition Definition
		//{
		//   get { return _webDeployDefinition; }
		//   set { _webDeployDefinition = value; }
		//}
	}

	public class WebDeployMessegaEventArgs : EventArgs
	{
		public string Message { get; set; }
		public TraceLevel Level { get; set; }
	}
}