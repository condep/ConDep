using System;
using System.Diagnostics;
using ConDep.Dsl.FluentWebDeploy.Deployment;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    [Binding]
    public class NServiceBusDefinition
    {
    	private NServiceBusExecutor _deployer;

    	[Given(@"I have fluently described how to deploy the NServiceBus project")]
        public void GivenIHaveFluentlyDescribedHowToDeployTheNServiceBusProject()
        {
            
        }

        [When(@"I create an instance of my class")]
        public void WhenICreateAnInstanceOfMyClass()
        {
        	_deployer = new NServiceBusExecutor();

        }

        [Then(@"the NServicebus project should successfully deploy")]
        public void ThenTheNServicebusProjectShouldSuccessfullyDeploy()
        {
            
        }
    }

    public class NServiceBusExecutor : WebDeployOperation
    {
        public NServiceBusExecutor()
        {
            Sync(s => s
                        .From.LocalHost()
                        .UsingProvider(p => p
                            .NServiceBus(@"C:\Temp\Frende.Customer.Endpoint", c => c
                                .ToDirectory(@"C:\Temp\Frende.Customer.Endpoint2")
							    .ServiceInstaller("NServiceBus.Host.exe")
							    .ServiceName("Frende.Customer.Endpoint")
							    .ServiceGroup("MyFrendeGroup")))
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
    }
}
