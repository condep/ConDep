using System;
using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
    public class WebDeploy
    {
        private readonly WebDeployDefinition _definition;
        private Action<object, WebDeployMessageEventArgs> _output;
        private Action<object, WebDeployMessageEventArgs> _outputError;

        public WebDeploy(WebDeployDefinition webDeployDefinition)
        {
            _definition = webDeployDefinition;
        }

        public DeploymentStatus Sync(Action<object, WebDeployMessageEventArgs> output, Action<object, WebDeployMessageEventArgs> outputError)
        {
            _output = output;
            _outputError = outputError;

            var deploymentStatus = new DeploymentStatus();
            WebDeployOptions options = null;

            try
            {
                options = ConfigureAllWebDeployOptions();

                foreach (var provider in _definition.Providers)
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
            }

            return deploymentStatus;
        }

        private void HandleSyncException(DeploymentStatus deploymentStatus, Exception ex)
        {
            deploymentStatus.AddUntrappedException(ex);
            var message = GetCompleteExceptionMessage(ex);

            if(_outputError != null)
            {
                _outputError(this, new WebDeployMessageEventArgs { Message = message, Level = TraceLevel.Error });
            }
        }

        WebDeployOptions ConfigureAllWebDeployOptions()
        {
            DeploymentBaseOptions destBaseOptions = null;

            var syncOptions = new DeploymentSyncOptions();
            var sourceBaseOptions = _definition.Source.GetSourceBaseOptions();

            destBaseOptions = _definition.Destination.GetDestinationBaseOptions();
            destBaseOptions.TempAgent = !_definition.Configuration.DoNotAutoDeployAgent;
            destBaseOptions.Trace += OnWebDeployTraceMessage;
            destBaseOptions.TraceLevel = TraceLevel.Verbose;

            return new WebDeployOptions(sourceBaseOptions, destBaseOptions, syncOptions);
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