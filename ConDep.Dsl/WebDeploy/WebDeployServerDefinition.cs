using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
    public class WebDeployServerDefinition : IValidate
	{
		private readonly WebDeploySource _webDeploySource = new WebDeploySource();
		private readonly WebDeployDestination _webDeployDestination = new WebDeployDestination();
		private readonly Configuration _configuration = new Configuration();
		private readonly List<IProvide> _providers = new List<IProvide>();

    	private EventHandler<WebDeployMessageEventArgs> _output;
    	private EventHandler<WebDeployMessageEventArgs> _outputError;

        public WebDeployServerDefinition() { }

        public WebDeployServerDefinition(DeploymentServer deploymentServer)
        {
            WebDeployDestination.ComputerName = deploymentServer.ServerName;
            WebDeploySource.LocalHost = true;

            if (!deploymentServer.User.IsDefined) return;

            WebDeployDestination.Credentials.UserName = deploymentServer.User.UserName;
            WebDeployDestination.Credentials.Password = deploymentServer.User.Password;

            WebDeploySource.Credentials.UserName = deploymentServer.User.UserName;
            WebDeploySource.Credentials.Password = deploymentServer.User.Password;
        }

        public WebDeploySource WebDeploySource
		{
			get { return _webDeploySource; }
		}

		public List<IProvide> Providers { get { return _providers; } }

		public WebDeployDestination WebDeployDestination
		{
			get { return _webDeployDestination; }
		}

		public Configuration Configuration
		{
			get { return _configuration; }
		}

        public TraceLevel TraceLevel { get; set; }

        public bool IsValid(Notification notification)
		{
			if (_providers.Count == 0) notification.AddError(new SemanticValidationError("No providers have been specified", ValidationErrorType.NoProviders));

			ValidateChildren(notification);

			return !notification.HasErrors;
		}

		private void ValidateChildren(Notification notification)
		{
			_webDeploySource.IsValid(notification);
			_webDeployDestination.IsValid(notification);
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
                      if(provider is WebDeployCompositeProviderBase)
                      {
                          ((WebDeployCompositeProviderBase) provider).BeforeExecute(output);
                      }
					  
                      provider.Sync(options, deploymentStatus);
                  
                      if (provider is WebDeployCompositeProviderBase)
                      {
                          ((WebDeployCompositeProviderBase)provider).AfterExecute(output);
                      }
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
			  var syncOptions = new DeploymentSyncOptions {WhatIf = Configuration.UseWhatIf};

		      var sourceBaseOptions = WebDeploySource.GetSourceBaseOptions();
		      sourceBaseOptions.TempAgent = !Configuration.DoNotAutoDeployAgent;
              sourceBaseOptions.Trace += OnWebDeployTraceMessage;
              sourceBaseOptions.TraceLevel = TraceLevel;

			  var destBaseOptions = WebDeployDestination.GetDestinationBaseOptions();
			  destBaseOptions.TempAgent = !Configuration.DoNotAutoDeployAgent;
			  destBaseOptions.Trace += OnWebDeployTraceMessage;
			  destBaseOptions.TraceLevel = TraceLevel;


			  return new WebDeployOptions(WebDeploySource.PackagePath, sourceBaseOptions, destBaseOptions, syncOptions);
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

		  private static string GetCompleteExceptionMessage(Exception exception)
		  {
			  var message = exception.Message;
			  if (exception.InnerException != null)
			  {
				  message += "\n" + GetCompleteExceptionMessage(exception.InnerException);
			  }
			  return message;
		  }

        private readonly Dictionary<string, WebDeployServerDefinition> ServerDefinitions = new Dictionary<string, WebDeployServerDefinition>();

        public static WebDeployServerDefinition CreateOrGetForServer(DeploymentServer deploymentServer)
        {
            //if (ServerDefinitions.ContainsKey(deploymentServer.ServerName))
            //{
            //    return ServerDefinitions[deploymentServer.ServerName];
            //}
            
            var definition = new WebDeployServerDefinition(deploymentServer);
            definition.ServerDefinitions.Add(deploymentServer.ServerName, definition);
            return definition;
        }
	}
}