using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ConDep.Dsl.Model.Config;
using Microsoft.Web.Deployment;
using System.Linq;

namespace ConDep.Dsl.WebDeploy
{
    public class WebDeployServerDefinition : IValidate
	{
		private readonly WebDeploySource _webDeploySource = new WebDeploySource();
		private readonly WebDeployDestination _webDeployDestination = new WebDeployDestination();
		private readonly Configuration _configuration = new Configuration();
		private readonly List<IProvide> _providers = new List<IProvide>();

        public WebDeployServerDefinition() { }

        public WebDeployServerDefinition(ServerConfig deploymentServer)
        {
            WebDeployDestination.ComputerName = deploymentServer.Name;
            WebDeploySource.LocalHost = true;

            if (!deploymentServer.DeploymentUser.IsDefined) return;

            WebDeployDestination.Credentials.UserName = deploymentServer.DeploymentUser.UserName;
            WebDeployDestination.Credentials.Password = deploymentServer.DeploymentUser.Password;

            //Todo: Should this user also be used for source?
            WebDeploySource.Credentials.UserName = deploymentServer.DeploymentUser.UserName;
            WebDeploySource.Credentials.Password = deploymentServer.DeploymentUser.Password;
        }

        public WebDeploySource WebDeploySource
		{
			get { return _webDeploySource; }
		}

		public IEnumerable<IProvide> Providers { get { return _providers; } }

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

		public WebDeploymentStatus Sync(WebDeploymentStatus deploymentStatus)
        {
			  WebDeployOptions options = null;

			  try
			  {
                  Logger.LogSectionStart(WebDeployDestination.ComputerName);
				  options = GetWebDeployOptions();

				  foreach (var provider in Providers)
				  {
                      if(provider is WebDeployCompositeProviderBase)
                      {
                          ((WebDeployCompositeProviderBase) provider).BeforeExecute();
                      }
					  
                      provider.Sync(options, deploymentStatus);
                  
                      if (provider is WebDeployCompositeProviderBase)
                      {
                          ((WebDeployCompositeProviderBase)provider).AfterExecute();
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

                  Logger.LogSectionEnd(WebDeployDestination.ComputerName);
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

              Logger.Error(message);
		  }

		  void OnWebDeployTraceMessage(object sender, DeploymentTraceEventArgs e)
		  {
			  if (e.EventLevel == TraceLevel.Error)
			  {
                  Logger.Error(e.Message);
			  }
              else if(e.EventLevel == TraceLevel.Warning)
              {
                  Logger.Warn(e.Message);
              }
			  else
			  {
			      Logger.Log(e.Message, TraceLevel.Verbose);
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

        public static WebDeployServerDefinition CreateOrGetForServer(ServerConfig deploymentServer)
        {
            //if (ServerDefinitions.ContainsKey(deploymentServer.ServerName))
            //{
            //    return ServerDefinitions[deploymentServer.ServerName];
            //}
            
            var definition = new WebDeployServerDefinition(deploymentServer);
            definition.ServerDefinitions.Add(deploymentServer.Name, definition);
            return definition;
        }

        public void AddProvider(IProvide provider, ConDepConfig envSettings)
        {
            if (envSettings != null && provider is IRequireCustomConfiguration)
            {
                AddCustomConfigForProvider(provider, envSettings);
            }
            _providers.Add(provider);
        }

        private void AddCustomConfigForProvider(IProvide provider, ConDepConfig envSettings)
        {
            var providerConfig = envSettings.CustomProviderConfig.FirstOrDefault(x => x.ProviderName == provider.GetType().Name);

            foreach(var property in providerConfig.ProviderConfig)
            {
                provider.GetType().GetProperty(property.Key).SetValue(provider, property.Value, null);
            }
        }
	}

    public interface IRequireCustomConfiguration
    {
    }
}