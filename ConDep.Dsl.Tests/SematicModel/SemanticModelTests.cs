using ConDep.Dsl.WebDeploy;
using Microsoft.Web.Deployment;
using NUnit.Framework;

namespace ConDep.Dsl.Tests.SematicModel
{
    //Todo:Unit test that warning is given when sites are listed in website.[env].env.js but not specified in Deployment dll
 
	public class when_no_providers_are_specified : SemanticTestFixture
	{
		[Test]
		public void should_notify_about_missing_provider()
		{
			Assert.That(Notification.HasErrorOfType(ValidationErrorType.NoProviders));
		}
	}
	
	public class when_no_configuration_are_specified : SemanticTestFixture
	{
		[Test]
		public void should_not_notify_about_missing_configuration()
		{
			Assert.That(!Notification.HasErrorOfType(ValidationErrorType.Configuration));
		}
	}

    public class when_no_source_computer_is_defined : SemanticTestFixture
    {
        private WebDeployServerDefinition _serverDefinition;
        private DeploymentBaseOptions _webDeployOptions;
        private Notification _notification;

        protected override void Given()
        {
            _serverDefinition = new WebDeployServerDefinition();
            _notification = new Notification();
        }

        protected override void When()
        {
            _webDeployOptions = _serverDefinition.WebDeploySource.GetSourceBaseOptions();
            _serverDefinition.WebDeploySource.IsValid(_notification);
        }

        [Test]
        public void should_default_to_localhost()
        {
            Assert.That(_serverDefinition.WebDeploySource.LocalHost, Is.True);
        }
    }

    public class when_destination_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_serverDefinition = new WebDeployServerDefinition();
			_serverDefinition.WebDeployDestination.ComputerName = COMPUTER_NAME;
			_serverDefinition.WebDeployDestination.Credentials.UserName = USERNAME;
			_serverDefinition.WebDeployDestination.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _serverDefinition.WebDeployDestination.GetDestinationBaseOptions();
			_serverDefinition.WebDeployDestination.IsValid(_notification);
		}
	}

	public class when_source_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_serverDefinition = new WebDeployServerDefinition();
			_serverDefinition.WebDeploySource.ComputerName = COMPUTER_NAME;
			_serverDefinition.WebDeploySource.Credentials.UserName = USERNAME;
			_serverDefinition.WebDeploySource.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _serverDefinition.WebDeploySource.GetSourceBaseOptions();
			_serverDefinition.WebDeploySource.IsValid(_notification);
		}
	}

    public class when_dont_know_yet : SemanticTestFixture
    {
        private WebDeploySetup _setup;
        private DeploymentServer _server;
        private Notification _notification;

        protected override void Given()
        {
            _server = new DeploymentServer("localhost", null);
            _setup = new WebDeploySetup();
        }

        protected override void When()
        {
            var definition = _setup.ConfigureServer(_server);
            definition.IsValid(_notification);
        }
    }

}