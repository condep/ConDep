using System;
using ConDep.Dsl.Core;
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
        private WebDeployDefinition _definition;
        private DeploymentBaseOptions _webDeployOptions;
        private Notification _notification;

        protected override void Given()
        {
            _definition = new WebDeployDefinition();
            _notification = new Notification();
        }

        protected override void When()
        {
            _webDeployOptions = _definition.WebDeploySource.GetSourceBaseOptions();
            _definition.WebDeploySource.IsValid(_notification);
        }

        [Test]
        public void should_default_to_localhost()
        {
            Assert.That(_definition.WebDeploySource.LocalHost, Is.True);
        }
    }

    public class when_destination_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_definition = new WebDeployDefinition();
			_definition.WebDeployDestination.ComputerName = COMPUTER_NAME;
			_definition.WebDeployDestination.Credentials.UserName = USERNAME;
			_definition.WebDeployDestination.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _definition.WebDeployDestination.GetDestinationBaseOptions();
			_definition.WebDeployDestination.IsValid(_notification);
		}
	}

	public class when_source_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_definition = new WebDeployDefinition();
			_definition.WebDeploySource.ComputerName = COMPUTER_NAME;
			_definition.WebDeploySource.Credentials.UserName = USERNAME;
			_definition.WebDeploySource.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _definition.WebDeploySource.GetSourceBaseOptions();
			_definition.WebDeploySource.IsValid(_notification);
		}
	}

}