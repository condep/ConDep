using System;
using System.Diagnostics;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeployHandler : IHandleWebDeploy
    {
        private ConDepUntrappedExitCodeException _untrappedExitCodeException;

        public IReportStatus Sync(IProvide provider, ServerConfig server, bool continueOnError, IReportStatus status, EventHandler<DeploymentTraceEventArgs> onTraceMessage)
        {
            _untrappedExitCodeException = null;
            var destBaseOptions = provider.GetWebDeployDestBaseOptions();

            try
            {
                var syncOptions = new DeploymentSyncOptions();

                var sourceBaseOptions = provider.GetWebDeploySourceBaseOptions();
                sourceBaseOptions.Trace += onTraceMessage;
                sourceBaseOptions.TraceLevel = TraceLevel.Verbose;

                destBaseOptions.Trace += onTraceMessage;
                destBaseOptions.TraceLevel = TraceLevel.Verbose;

                destBaseOptions.ComputerName = server.WebDeployAgentUrl;
                destBaseOptions.UserName = server.DeploymentUser.UserName;
                destBaseOptions.Password = server.DeploymentUser.Password;


                var defaultWaitInterval = destBaseOptions.RetryInterval;
                var defaultRetryAttempts = destBaseOptions.RetryAttempts;

                if (provider.WaitIntervalInSeconds > 0)
                {
                    destBaseOptions.RetryInterval = provider.WaitIntervalInSeconds * 1000;
                }

                if (provider.RetryAttempts > 0)
                {
                    destBaseOptions.RetryAttempts = provider.RetryAttempts;
                }

                destBaseOptions.Trace += CheckForUntrappedExitCodes;

                DeploymentChangeSummary summery;
                using (var sourceDepObject = DeploymentManager.CreateObject(provider.GetWebDeploySourceProviderOptions(), sourceBaseOptions))
                {
                    foreach (var rule in provider.GetReplaceRules())
                    {
                        syncOptions.Rules.Add(rule);
                    }

                    summery = sourceDepObject.SyncTo(provider.GetWebDeployDestinationProviderOptions(), destBaseOptions, syncOptions);
                }

                status.AddSummery(summery);

                destBaseOptions.RetryInterval = defaultWaitInterval;
                destBaseOptions.RetryAttempts = defaultRetryAttempts;

                if (summery.Errors > 0)
                {
                    throw new ConDepWebDeployProviderException("The provider reported " + summery.Errors + " during deployment.");
                }
            }
            catch
            {
                if (!continueOnError)
                {
                    throw;
                }
            }
            finally
            {
                destBaseOptions.Trace -= CheckForUntrappedExitCodes;

                if (_untrappedExitCodeException != null && !continueOnError)
                {
                    throw _untrappedExitCodeException;
                }
            }
            return status;
        }

        void CheckForUntrappedExitCodes(object sender, DeploymentTraceEventArgs e)
        {
            //Terrible hack to trap exit codes that the WebDeploy runCommand ignores!
            if (e.Message.Contains("exited with code "))
            {
                if (!e.Message.Contains("exited with code '0x0'"))
                {
                    _untrappedExitCodeException = new ConDepUntrappedExitCodeException(e.Message, _untrappedExitCodeException);
                }
            }
        }
    }
}