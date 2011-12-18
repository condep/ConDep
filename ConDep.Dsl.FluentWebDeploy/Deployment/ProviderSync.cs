using System;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
    public class ProviderSync
    {
        private Exception _untrappedExitCodeException;

        public DeploymentStatus Execute(IProvide provider, WebDeployOptions webDeployOptions, DeploymentStatus deploymentStatus)
        {
            if (provider is Provider)
            {
                SyncWebDeployProvider(provider, webDeployOptions, deploymentStatus);
            }
            else if (provider is CompositeProvider)
            {
                SyncCompositeProvider(provider, webDeployOptions, deploymentStatus);
            }
            else
            {
                throw new Exception(string.Format("Provider type <{0}> not supported.", provider.GetType().Name));
            }

            return deploymentStatus;
        }

        private void SyncWebDeployProvider(IProvide provider, WebDeployOptions options, DeploymentStatus deploymentStatus)
        {
            try
            {
                options.DestBaseOptions.Trace += CheckForUntrappedRunCommandExitCodes;
                var defaultWaitInterval = options.DestBaseOptions.RetryInterval;

                if (provider.WaitInterval > 0)
                {
                    options.DestBaseOptions.RetryInterval = provider.WaitInterval * 1000;
                }

                var sourceDepObject = ((Provider)provider).GetWebDeploySourceObject(options.SourceBaseOptions);
                var destProviderOptions = ((Provider)provider).GetWebDeployDestinationObject();

                var summery = sourceDepObject.SyncTo(destProviderOptions, options.DestBaseOptions, options.SyncOptions);

                deploymentStatus.AddSummery(summery);

                options.DestBaseOptions.RetryInterval = defaultWaitInterval;

                if (summery.Errors > 0)
                {
                    throw new Exception("The provider reported " + summery.Errors + " during deployment.");
                }

                if (_untrappedExitCodeException != null)
                {
                    throw _untrappedExitCodeException;
                }
            }
            finally
            {
                options.DestBaseOptions.Trace -= CheckForUntrappedRunCommandExitCodes;
            }

        }

        private void SyncCompositeProvider(IProvide provider, WebDeployOptions options, DeploymentStatus deploymentStatus)
        {
            foreach (var childProvider in ((CompositeProvider)provider).ChildProviders)
            {
                if (childProvider is CompositeProvider)
                {
                    SyncCompositeProvider(childProvider, options, deploymentStatus);
                }
                else
                {
                    SyncWebDeployProvider(childProvider, options, deploymentStatus);
                }
            }
        }

        void CheckForUntrappedRunCommandExitCodes(object sender, DeploymentTraceEventArgs e)
        {
            //Terrible hack to trap exit codes that the WebDeploy runCommand ignores!
            if (e.Message.Contains("exited with code "))
            {
                if (!e.Message.Contains("exited with code '0x0'"))
                {
                    _untrappedExitCodeException = new Exception(e.Message, _untrappedExitCodeException);
                }
            }
        }

    }
}