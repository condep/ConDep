using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeploySyncronizer
    {
        private ConDepUntrappedExitCodeException _untrappedExitCodeException;

        public virtual IReportStatus Sync(IProvide provider, WebDeployOptions webDeployOptions, bool continueOnError, IReportStatus status)
        {
            try
            {
                var defaultWaitInterval = webDeployOptions.DestBaseOptions.RetryInterval;
                var defaultRetryAttempts = webDeployOptions.DestBaseOptions.RetryAttempts;

                webDeployOptions.DestBaseOptions.Trace += CheckForUntrappedExitCodes;

                if (WaitInterval > 0)
                {
                    webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
                }

                if (RetryAttempts > 0)
                {
                    webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;
                }

                DeploymentChangeSummary summery;
                using (var sourceDepObject = webDeployOptions.FromPackage ? GetPackageSourceObject(webDeployOptions) : provider.GetWebDeploySourceObject(webDeployOptions.SourceBaseOptions))
                {
                    var destProviderOptions = provider.GetWebDeployDestinationObject();

                    foreach (var rule in provider.GetReplaceRules())
                    {
                        webDeployOptions.SyncOptions.Rules.Add(rule);
                    }
                    summery = sourceDepObject.SyncTo(destProviderOptions, webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
                }

                status.AddSummery(summery);

                webDeployOptions.DestBaseOptions.RetryInterval = defaultWaitInterval;
                webDeployOptions.DestBaseOptions.RetryAttempts = defaultRetryAttempts;

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
                webDeployOptions.DestBaseOptions.Trace -= CheckForUntrappedExitCodes;

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

        public static DeploymentObject GetPackageSourceObject(WebDeployOptions webDeployOptions)
        {
            return DeploymentManager.CreateObject(DeploymentWellKnownProvider.Package, webDeployOptions.PackagePath, webDeployOptions.SourceBaseOptions);
        }


        protected int RetryAttempts { get; set; }
        protected int WaitInterval { get; set; }
    }
}