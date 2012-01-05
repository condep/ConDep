using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using ConDep.Dsl.Operations.WebDeploy.Model;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.Specs
{
    [Binding]
    public class StepDefinition
    {
        private string _command;
        private string _provider;
        private string _certificateThumbprint;
        private WebDeploymentStatus _deploymentStatus;


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
            IExecuteWebDeploy executor;

            switch (_provider.ToLower())
            {
                case "powershell":
                    executor = new PowerShellExecutor(_command);
                    _deploymentStatus = executor.Execute();
                    break;
                case "runcommand":
                    executor = new RunCmdExecutor(_command);
                    _deploymentStatus = executor.Execute();
                    break;
                case "certificate":
                    executor = new CertificateExecutor(_certificateThumbprint);
                    _deploymentStatus = executor.Execute();
                    break;
                case "website":
                    executor = new WebSiteExecutor();
                    _deploymentStatus = executor.Execute();
                    break;
                default:
                    throw new Exception("Provider not known!");
            }
        }

        [When(@"I deploy from package")]
        public void WhenIDeployFromPackage()
        {
            IExecuteWebDeploy executor;

            switch (_provider.ToLower())
            {
                case "powershell":
                    executor = new PowerShellExecutor(_command);
                    _deploymentStatus = executor.ExecuteFromPackage();
                    break;
                case "runcommand":
                    executor = new RunCmdExecutor(_command);
                    _deploymentStatus = executor.ExecuteFromPackage();
                    break;
                case "certificate":
                    executor = new CertificateExecutor(_certificateThumbprint);
                    _deploymentStatus = executor.ExecuteFromPackage();
                    break;
                default:
                    throw new Exception("Provider not known!");
            }
        }

        [Then(@"I would expect the certificate with thumbprint (.*) to be found in the cert store")]
        public void ThenIWouldExpectTheCertificateToBeFoundInTheCertStore(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            
            Assert.That(certs.Count, Is.EqualTo(1));

            store.Close();
        }


        [Then(@"I would expect no errors")]
        public void ThenIWouldExpectNoErrors()
        {
            Assert.That(_deploymentStatus.HasErrors, Is.False);
        }

        [Then(@"I would expect an exit code error")]
        public void ThenIWouldExpectAnExitCodeError()
        {
            Assert.That(_deploymentStatus.HasExitCodeErrors, Is.True);
        }

        [Then(@"an exception should occour")]
        public void ThenAnExceptionShouldOccour()
        {
            Assert.That(_deploymentStatus.HasErrors);
        }
    }

    public class WebSiteExecutor : ConDepOperation, IExecuteWebDeploy
    {
        public WebDeploymentStatus Execute()
        {
            return Setup(setup => setup.WebDeploy(s => s
                                                           .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                                           .From.LocalHost()
                                                           .UsingProvider(p => p
                                                                                   .WebSite("Default Web Site",
                                                                                            "Default Web Site 2")
                                                                                   .Exclude.AppPools()
                                                                                   .Certificates()
                                                                                   .CertificatesOnIisBindings()
                                                                                   .Content()
                                                                                   .FrameworkConfig())
                                                           .To.LocalHost()
                                      ));
        }

        public WebDeploymentStatus ExecuteFromPackage()
        {
            throw new NotImplementedException();
        }

        protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }
    }

    public class CertificateExecutor : ConDepOperation, IExecuteWebDeploy
    {
        private readonly string _certificateThumbprint;

        public CertificateExecutor(string certificateThumbprint)
        {
            _certificateThumbprint = certificateThumbprint;
        }

        public WebDeploymentStatus ExecuteFromPackage()
        {
            return Setup(setup => setup.WebDeploy(s => s
                                                           .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                                           .From.Package(@"C:\package.zip", "test123")
                                                           .UsingProvider(p => p
                                                                                   .Certificate(_certificateThumbprint))
                                                           .To.LocalHost()
                                      ));

        }

        public WebDeploymentStatus Execute()
        {
            return Setup(setup => setup.WebDeploy(s => s
                                                           .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                                           .From.LocalHost()
                                                           .UsingProvider(p => p
                                                                                   .Certificate(_certificateThumbprint))
                                                           .To.LocalHost()
                                      ));
        }

        protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }
    }

    public interface IExecuteWebDeploy
    {
        WebDeploymentStatus Execute();
        WebDeploymentStatus ExecuteFromPackage();
    }

    public class RunCmdExecutor : ConDepOperation, IExecuteWebDeploy
    {
        private readonly string _command;

        public RunCmdExecutor(string command)
        {
            _command = command;
        }

        protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceError(e.Message);
        }

        public WebDeploymentStatus Execute()
        {
            return Setup(setup => setup.WebDeploy(s => s
                                                           .WithConfiguration(c => c.DoNotAutoDeployAgent())
                                                           .From.LocalHost()
                                                           .UsingProvider(p => p
                                                                                   .RunCmd(_command))
                                                           .To.LocalHost()
                                      ));

        }

        public WebDeploymentStatus ExecuteFromPackage()
        {
            throw new NotImplementedException();
        }
    }
}
