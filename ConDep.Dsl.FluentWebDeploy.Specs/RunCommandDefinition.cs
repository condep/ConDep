using System;
using System.Diagnostics;
using System.ServiceProcess;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    [Binding]
    public class RunCommandDefinition
    {
        private string _command;
        private RunCmdExecutor _executor;

        [Given(@"the WebDeploy Agent Service is running")]
        public void GivenTheWebDeployAgentServiceIsRunning()
        {
            var controller = new ServiceController("MsDepSvc");
            if(controller.Status != ServiceControllerStatus.Running)
            {
                throw new Exception("The WebDeploy Agent Service is not running.");
            }
        }

        [Given(@"I have entered the command (.*)")]
        public void GivenIHaveEnteredTheCommandDateT(string command)
        {
            _command = command;
        }

        [When(@"I execute my Run Command")]
        public void WhenIExecuteMyRunCommand()
        {
            _executor = new RunCmdExecutor(_command);
        }

        [Then(@"I would expect no errors")]
        public void ThenIWouldExpectNoErrors()
        {
            Assert.That(_executor.DeploymentStatus.HasErrors, Is.False);
        }

        [Then(@"I would expect an exit code error")]
        public void ThenIWouldExpectAnExitCodeError()
        {
            Assert.That(_executor.DeploymentStatus.HasExitCodeErrors, Is.True);
        }
    }

    public class RunCmdExecutor : WebDeployOperation
    {
        private readonly DeploymentStatus _deploymentStatus;

        public RunCmdExecutor(string command)
        {
            _deploymentStatus = Sync(s => s
                        .WithConfiguration(c => c.DoNotAutoDeployAgent())
                        .From.LocalHost()
                        .UsingProvider(p => p
                            .RunCmd(command))
                        .To.LocalHost()
                     );
        }
        protected override void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceError(e.Message);
        }

        public DeploymentStatus DeploymentStatus
        {
            get { return _deploymentStatus; }
        }

    }

}
