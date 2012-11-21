using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Experimental.Core
{
    public class RemoteOperation : IOperateRemote
    {
        private readonly IProvide _provider;
        private readonly ILogForConDep _logger;
        private readonly IOperateWebDeploy _webDeploy;

        public RemoteOperation(IProvide provider, ILogForConDep logger, IOperateWebDeploy webDeploy)
        {
            _provider = provider;
            _logger = logger;
            _webDeploy = webDeploy;
        }

        public bool IsValid(Notification notification)
        {
            return _provider.IsValid(notification);
        }

        public IReportStatus Execute(ServerConfig server, IReportStatus status)
        {
            WebDeployOptions options = null;
            try
            {
                _logger.LogSectionStart(server.Name);
                options = _webDeploy.GetWebDeployOptions(server, OnWebDeployTraceMessage);//GetWebDeployOptions(webDeploySource, webDeployDestination);

                if (_provider is WebDeployCompositeProviderBase)
                {
                    ((WebDeployCompositeProviderBase)_provider).BeforeExecute();
                }

                _provider.Sync(options, status);

                if (_provider is WebDeployCompositeProviderBase)
                {
                    ((WebDeployCompositeProviderBase)_provider).AfterExecute();
                }
            }
            catch (Exception ex)
            {
                HandleSyncException(status, ex);
            }
            finally
            {
                if (options != null && options.DestBaseOptions != null) options.DestBaseOptions.Trace -= OnWebDeployTraceMessage;
                if (options != null && options.SourceBaseOptions != null) options.SourceBaseOptions.Trace -= OnWebDeployTraceMessage;

                _logger.LogSectionEnd(server.Name);
            }

            return status;
        }

        private void HandleSyncException(IReportStatus status, Exception ex)
        {
            status.AddUntrappedException(ex);
            var message = GetCompleteExceptionMessage(ex);

            _logger.Error(message);
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

        private void OnWebDeployTraceMessage(object sender, DeploymentTraceEventArgs e)
        {
            if (e.EventLevel == TraceLevel.Error)
            {
                _logger.Error(e.Message);
            }
            else if (e.EventLevel == TraceLevel.Warning)
            {
                _logger.Warn(e.Message);
            }
            else
            {
                _logger.Log(e.Message, TraceLevel.Verbose);
            }
        }
    }

    public interface IReportStatus
    {
        void AddSummery(DeploymentChangeSummary summery);
        bool HasErrors { get; }
        bool HasExitCodeErrors { get; }
        void AddUntrappedException(Exception exception);
        void AddConditionMessage(string message);
    }

    public class StatusReporter : IReportStatus
    {
        private readonly List<DeploymentChangeSummary> _summeries = new List<DeploymentChangeSummary>();
        private readonly List<Exception> _untrappedExceptions = new List<Exception>();
        private readonly List<string> _conditionMessages = new List<string>();

        public void AddSummery(DeploymentChangeSummary summery)
        {
            _summeries.Add(summery);
        }

        public bool HasErrors
        {
            get
            {
                return _summeries.Any(s => s.Errors > 0) || _untrappedExceptions.Count > 0;
            }
        }

        public void AddUntrappedException(Exception exception)
        {
            _untrappedExceptions.Add(exception);
        }

        public bool HasExitCodeErrors
        {
            get { return _untrappedExceptions.Count > 0; }
        }

        public void AddConditionMessage(string message)
        {
            _conditionMessages.Add(message);
        }


    }

}