using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Specs.Executors;
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
}
