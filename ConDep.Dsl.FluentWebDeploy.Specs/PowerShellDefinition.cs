using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.Deployment;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    [Binding]
    public class PowerShellDefinition
    {
        private string _command;
        private PowerShellExecutor _executor;

        [Given(@"I have provided the powershell command (.*) to the powershell provider")]
        public void GivenIHaveProvidedThePowershellCommandGet_DateToThePowershellProvider(string command)
        {
            _command = command;
        }
        
        [When(@"I execute my DSL")]
        public void WhenIExecuteMyDSL()
        {
            _executor = new PowerShellExecutor(_command);
        }

        [Then(@"an exception should occour")]
        public void ThenAnExceptionShouldOccour()
        {
            Assert.That(_executor.DeploymentStatus.HasErrors);
        }

    }

    public class PowerShellExecutor : WebDeployOperation
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
