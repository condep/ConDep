using ConDep.WebDeploy.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests.SematicModel
{
	public abstract class SemanticTestFixture : SimpleTestFixture
	{
		private WebDeployDefinition _definition;

		protected override void Given()
		{
			_definition = new WebDeployDefinition();
			Notification = new Notification();
		}

		protected override void When()
		{
			_definition.IsValid(Notification);
		}

		protected Notification Notification { get; private set; }
	}

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
}