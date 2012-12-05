using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
	public class RunCmdProvider : WebDeployProviderBase
	{
	    private readonly bool _continueOnError;
        private ConDepUntrappedExitCodeException _untrappedExitCodeException;
        private const string NAME = "runCommand";

		public RunCmdProvider(string command, bool continueOnError)
		{
		    _continueOnError = continueOnError;
		    DestinationPath = command;
		}

	    public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, "", sourceBaseOptions);
		}

		public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
		{
			var destProviderOptions = new DeploymentProviderOptions(Name) { Path = DestinationPath };
            //DeploymentProviderSetting dontUseCmdExe;
            //if (destProviderOptions.ProviderSettings.TryGetValue("dontUseCommandExe", out dontUseCmdExe))
            //{
            //    dontUseCmdExe.Value = true;
            //}
		    return destProviderOptions;
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(DestinationPath) ||
                string.IsNullOrWhiteSpace(SourcePath);
		}

        public override IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status)
        {
            try
            {
                webDeployOptions.DestBaseOptions.Trace += CheckForUntrappedRunCommandExitCodes;
                base.Sync(webDeployOptions, status);
            }
            catch
            {
                if(!_continueOnError)
                {
                    throw;
                }
            }
            finally
            {
                webDeployOptions.DestBaseOptions.Trace -= CheckForUntrappedRunCommandExitCodes;

                if (_untrappedExitCodeException != null && !_continueOnError)
                {
                    throw _untrappedExitCodeException;
                }
            }
            return status;
        }

        void CheckForUntrappedRunCommandExitCodes(object sender, DeploymentTraceEventArgs e)
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