using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    public class PowerShellExecutor : WebDeployOperation, IExecuteWebDeploy
    {
        private readonly DeploymentStatus _deploymentStatus;

        public PowerShellExecutor(string command)
        {
            _deploymentStatus = Sync(s => s
                                              .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                              .From.LocalHost()
                                              .UsingProvider(p => p
                                                                      .PowerShell(command))
                                              .To.LocalHost()
                );
        }

        public DeploymentStatus DeploymentStatus
        {
            get { return _deploymentStatus; }
        }

        protected override void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceError(e.Message);
        }
    }

}
