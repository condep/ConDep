using System;
using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    public class PowerShellExecutor : WebDeployOperation, IExecuteWebDeploy
    {
        private readonly string _command;

        public PowerShellExecutor(string command)
        {
            _command = command;
        }

        protected override void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceError(e.Message);
        }

        public DeploymentStatus Execute()
        {
            return Sync(s => s
                                              .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                              .From.LocalHost()
                                              .UsingProvider(p => p
                                                                      .PowerShell(_command))
                                              .To.LocalHost()
                );
        }

        public DeploymentStatus ExecuteFromPackage()
        {
            throw new NotImplementedException();
        }
    }

}
