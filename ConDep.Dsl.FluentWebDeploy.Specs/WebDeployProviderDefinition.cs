using System;
using System.Diagnostics;
using System.ServiceProcess;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    [Binding]
    public class WebDeployProviderDefinition
    {
        private string _command;
        private IExecuteWebDeploy _executor;
        private string _provider;
        private string _certificateThumbprint;

        [Given(@"the WebDeploy Agent Service is running")]
        public void GivenTheWebDeployAgentServiceIsRunning()
        {
            using(var controller = new ServiceController("MsDepSvc"))
            {
                if (controller.Status != ServiceControllerStatus.Running)
                {
                    throw new Exception("The WebDeploy Agent Service is not running.");
                }
            }
        }

        [Given(@"I have entered the command (.*)")]
        public void GivenIHaveEnteredTheCommandDateT(string command)
        {
            _command = command;
        }

        [Given(@"I am using the (.*) provider")]
        public void GivenIAmUsingTheProvider(string provider)
        {
            _provider = provider;
        }

        [Given(@"I have entered the certificate thumbprint (.*)")]
        public void GivenIHaveEnteredTheCertificateThumbprint(string thumbprint)
        {
            _certificateThumbprint = string.Format(@"my\{0}",thumbprint.Trim(' '));
        }


        [When(@"I execute my DSL")]
        public void WhenIExecuteMyDSL()
        {
            switch (_provider.ToLower())
            {
                case "powershell":
                    _executor = new PowerShellExecutor(_command);
                    break;
                case "runcommand":
                    _executor = new RunCmdExecutor(_command);
                    break;
                case "certificate":
                    _executor = new CertificateExecutor(_certificateThumbprint);
                    break;
                default:
                    throw new Exception("Provider not known!");
            }
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

        [Then(@"an exception should occour")]
        public void ThenAnExceptionShouldOccour()
        {
            Assert.That(_executor.DeploymentStatus.HasErrors);
        }
    }

    public class CertificateExecutor : WebDeployOperation, IExecuteWebDeploy
    {
        private readonly DeploymentStatus _deploymentStatus;

        public CertificateExecutor(string certificateThumbprint)
        {
            _deploymentStatus = Sync(s => s
                        .WithConfiguration(c => c.DoNotAutoDeployAgent())
                        .From.Package(@"C:\package.zip")
                        .UsingProvider(p => p
                            .Certificate(certificateThumbprint))
                        .To.LocalHost()
                     );
        }

        protected override void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        public DeploymentStatus DeploymentStatus
        {
            get { return _deploymentStatus; }
        }
    }

    public interface IExecuteWebDeploy
    {
        DeploymentStatus DeploymentStatus { get; }
    }

    public class RunCmdExecutor : WebDeployOperation, IExecuteWebDeploy
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
