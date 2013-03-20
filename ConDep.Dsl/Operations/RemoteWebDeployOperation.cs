using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations
{
    public class RemoteWebDeployOperation : IOperateRemote
    {
        private readonly IProvide _provider;
        private readonly IHandleWebDeploy _webDeploy;

        public RemoteWebDeployOperation(IProvide provider, IHandleWebDeploy webDeploy)
        {
            _provider = provider;
            _webDeploy = webDeploy;
        }

        public bool IsValid(Notification notification)
        {
            return _provider.IsValid(notification);
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            try
            {
                Logger.LogSectionStart(_provider.GetType().Name);
                _webDeploy.Sync(_provider, server, _provider.ContinueOnError, status, OnWebDeployTraceMessage);
            }
            catch (Exception ex)
            {
                HandleSyncException(status, ex);
            }
            finally
            {
                Logger.LogSectionEnd(_provider.GetType().Name);
            }
        }

        private void HandleSyncException(IReportStatus status, Exception ex)
        {
            status.AddUntrappedException(ex);
            var message = GetCompleteExceptionMessage(ex);

            Logger.Error(message);
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
            if (e.Message.Contains("exited with code "))
            {
                if (e.Message.Contains("exited with code '0x0'"))
                {
                    Logger.Verbose(e.Message);
                }
                else
                {
                    Logger.Error(e.Message);
                }
            }
            else
            {
                Logger.Log(e.Message, e.EventLevel);
            }
        }
    }
}