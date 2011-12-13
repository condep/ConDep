using System;
using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
	public class WebDeploy : IWebDeploy
	{
		public event EventHandler<WebDeployMessageEventArgs> Output;
		public event EventHandler<WebDeployMessageEventArgs> OutputError;

		public void Deploy(WebDeployDefinition webDeployDefinition)
		{
			try
			{
				var syncOptions = new DeploymentSyncOptions();
				var sourceBaseOptions = webDeployDefinition.Source.GetSourceBaseOptions();
				var destBaseOptions = webDeployDefinition.Destination.GetDestinationBaseOptions();

				destBaseOptions.Trace += OnWebDeployTraceMessage;
				destBaseOptions.TraceLevel = TraceLevel.Verbose;

                if (webDeployDefinition.Configuration.AutoDeployAgent)
                {
                    destBaseOptions.TempAgent = true;
                }

				foreach (var provider in webDeployDefinition.Providers)
				{
                    if(provider is Provider)
                    {
                        SyncProvider(destBaseOptions, provider, sourceBaseOptions, syncOptions);
                    }
                    else if(provider is CompositeProvider)
                    {
                        foreach(var childProvider in ((CompositeProvider)provider).ChildProviders)
                        {
                            SyncProvider(destBaseOptions, childProvider, sourceBaseOptions, syncOptions);
                        }
                    }
				}

				destBaseOptions.Trace -= OnWebDeployTraceMessage;
			}
			catch(Exception ex)
			{
				if(OutputError != null)
				{
					var message = GetCompleteExceptionMessage(ex);

					OutputError(this, new WebDeployMessageEventArgs { Message = message, Level = TraceLevel.Error });
				}
				else
				{
					throw;
				}
			}
		}

	    private void SyncProvider(DeploymentBaseOptions destBaseOptions, IProvide provider, DeploymentBaseOptions sourceBaseOptions, DeploymentSyncOptions syncOptions)
	    {
	        var sourceDepObject = ((Provider)provider).GetWebDeploySourceObject(sourceBaseOptions);
	        var destProviderOptions = ((Provider)provider).GetWebDeployDestinationObject();

	        sourceDepObject.SyncTo(destProviderOptions, destBaseOptions, syncOptions);
	    }

	    private string GetCompleteExceptionMessage(Exception exception)
		{
			var message = exception.Message;
			if (exception.InnerException != null)
			{
				message += "\n" + GetCompleteExceptionMessage(exception.InnerException);
			}
			return message;
		}

		void OnWebDeployTraceMessage(object sender, DeploymentTraceEventArgs e)
		{
			if(e.EventLevel == TraceLevel.Error)
			{
				if(OutputError != null)
				{
					OutputError(this, new WebDeployMessageEventArgs {Message = e.Message, Level = e.EventLevel});
				}
			}
			else
			{
				if (Output != null)
				{
					Output(this, new WebDeployMessageEventArgs { Message = e.Message, Level = e.EventLevel });
				}
			}
		}

		public void Delete(WebDeployDefinition webDeployDefinition)
		{
			throw new NotImplementedException();
		}
	}
}