using System;
using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    /*Todo:
     * 1) Check if MSMQ exist before running integration tests
     * 2) Use a simple nservicebus implementation to test installation
    */
    [Binding]
    public class NServiceBusDefinition
    {
    	[Given(@"I have fluently described how to deploy the NServiceBus project")]
        public void GivenIHaveFluentlyDescribedHowToDeployTheNServiceBusProject()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I create an instance of my class")]
        public void WhenICreateAnInstanceOfMyClass()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the NServicebus project should successfully deploy")]
        public void ThenTheNServicebusProjectShouldSuccessfullyDeploy()
        {
            ScenarioContext.Current.Pending();
        }
    }

    public class NServiceBusExecutor : ConDepOperation
    {
        private readonly WebDeploymentStatus _deploymentStatus;

        public NServiceBusExecutor()
        {
        	_deploymentStatus = Setup(setup => setup.WebDeploy(s => s
        	                                                        	.From.LocalHost()
        	                                                        	.UsingProvider(p => p
        	                                                        	                    	.NServiceBus(
        	                                                        	                    		@"C:\Temp\Frende.Customer.Endpoint",
        	                                                        	                    		"Frende.Customer.Endpoint",
        	                                                        	                    		c => c
        	                                                        	                    		     	.DestinationDir(
        	                                                        	                    		     		@"C:\Temp\Frende.Customer.Endpoint2")
        	                                                        	                    		     	.ServiceInstaller(
        	                                                        	                    		     		"NServiceBus.Host.exe")
        	                                                        	                    		     	.ServiceGroup(
        	                                                        	                    		     		"MyFrendeGroup")))
        	                                                        	.To.LocalHost()
        	                                   	));
        }

        public WebDeploymentStatus DeploymentStatus
        {
            get { return _deploymentStatus; }
        }

        protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
        {
        	Trace.TraceInformation(e.Message);
        }

        protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
        	Trace.TraceError(e.Message);
        }
    }
}
