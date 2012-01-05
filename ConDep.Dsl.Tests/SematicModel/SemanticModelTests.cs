using ConDep.Dsl.Operations.WebDeploy.Model;
using NUnit.Framework;

namespace ConDep.Dsl.Tests.SematicModel
{
	public class when_no_providers_are_specified : SemanticTestFixture
	{
		[Test]

		public void should_notify_about_missing_provider()
		{
			Assert.That(Notification.HasErrorOfType(ValidationErrorType.NoProviders));
		}
	}
	
	public class when_no_source_are_specified : SemanticTestFixture
	{
		[Test]
		public void should_notify_about_missing_source()
		{
			Assert.That(Notification.HasErrorOfType(ValidationErrorType.NoSource));
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

	public class when_destination_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_definition = new WebDeployDefinition();
			_definition.Destination.ComputerName = COMPUTER_NAME;
			_definition.Destination.Credentials.UserName = USERNAME;
			_definition.Destination.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _definition.Destination.GetDestinationBaseOptions();
			_definition.Destination.IsValid(_notification);
		}
	}

	public class when_source_is_defined_correct : WebDeployOptionsTestFixture
	{
		protected override void Given()
		{
			_definition = new WebDeployDefinition();
			_definition.Source.ComputerName = COMPUTER_NAME;
			_definition.Source.Credentials.UserName = USERNAME;
			_definition.Source.Credentials.Password = PASSWORD;

			_notification = new Notification();
		}

		protected override void When()
		{
			_webDeployOptions = _definition.Source.GetSourceBaseOptions();
			_definition.Source.IsValid(_notification);
		}
	}

}