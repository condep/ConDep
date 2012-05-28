using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.WebDeploy.Model
{
    public class WebDeployDefinition : IValidate
	{
		private readonly Source _source = new Source();
		private readonly Destination _destination = new Destination();
		private readonly Configuration _configuration = new Configuration();
		private readonly List<IProvide> _providers = new List<IProvide>();

    	private EventHandler<WebDeployMessageEventArgs> _output;
    	private EventHandler<WebDeployMessageEventArgs> _outputError;

        public Source Source
		{
			get { return _source; }
		}

		public List<IProvide> Providers { get { return _providers; } }

		public Destination Destination
		{
			get { return _destination; }
		}

		public Configuration Configuration
		{
			get { return _configuration; }
		}

		public bool IsValid(Notification notification)
		{
			if (_providers.Count == 0) notification.AddError(new SemanticValidationError("No providers have been specified", ValidationErrorType.NoProviders));

			ValidateChildren(notification);

			return !notification.HasErrors;
		}

		private void ValidateChildren(Notification notification)
		{
			_source.IsValid(notification);
			_destination.IsValid(notification);
			_configuration.IsValid(notification);
			_providers.ForEach(p => p.IsValid(notification));
		}

		public WebDeploymentStatus Sync(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus deploymentStatus)
        {
			  _output = output;
			  _outputError = outputError;

			  WebDeployOptions options = null;

			  try
			  {
				  options = GetWebDeployOptions();

				  foreach (var provider in Providers)
				  {
					  provider.Sync(options, deploymentStatus);
				  }
			  }
			  catch (Exception ex)
			  {
				  HandleSyncException(deploymentStatus, ex);
			  }
			  finally
			  {
                  if (options != null && options.DestBaseOptions != null) options.DestBaseOptions.Trace -= OnWebDeployTraceMessage;
                  if (options != null && options.SourceBaseOptions != null) options.SourceBaseOptions.Trace -= OnWebDeployTraceMessage;
              }

			  return deploymentStatus;
		  }

		  WebDeployOptions GetWebDeployOptions()
		  {
			  DeploymentBaseOptions destBaseOptions = null;

			  var syncOptions = new DeploymentSyncOptions {WhatIf = Configuration.UseWhatIf};

		      var sourceBaseOptions = Source.GetSourceBaseOptions();
		      sourceBaseOptions.TempAgent = !Configuration.DoNotAutoDeployAgent;
              sourceBaseOptions.Trace += OnWebDeployTraceMessage;
              sourceBaseOptions.TraceLevel = TraceLevel.Verbose;

			  destBaseOptions = Destination.GetDestinationBaseOptions();
			  destBaseOptions.TempAgent = !Configuration.DoNotAutoDeployAgent;
			  destBaseOptions.Trace += OnWebDeployTraceMessage;
			  destBaseOptions.TraceLevel = TraceLevel.Verbose;


			  return new WebDeployOptions(Source.PackagePath, sourceBaseOptions, destBaseOptions, syncOptions);
		  }

		  private void HandleSyncException(WebDeploymentStatus deploymentStatus, Exception ex)
		  {
			  deploymentStatus.AddUntrappedException(ex);
			  var message = GetCompleteExceptionMessage(ex);

			  if (_outputError != null)
			  {
				  _outputError(this, new WebDeployMessageEventArgs { Message = message, Level = TraceLevel.Error });
			  }
		  }

		  void OnWebDeployTraceMessage(object sender, DeploymentTraceEventArgs e)
		  {
			  if (e.EventLevel == TraceLevel.Error)
			  {
				  if (_outputError != null)
				  {
					  _outputError(this, new WebDeployMessageEventArgs { Message = e.Message, Level = e.EventLevel });
				  }
			  }
			  else
			  {
				  if (_output != null)
				  {
					  _output(this, new WebDeployMessageEventArgs { Message = e.Message, Level = e.EventLevel });
				  }
			  }
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

	}
}