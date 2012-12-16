using ConDep.Dsl;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
	public abstract class WebDeployOptionsTestFixture : SimpleTestFixtureBase
	{
        //protected WebDeployServerDefinition _serverDefinition;
		protected DeploymentBaseOptions _webDeployOptions;
		protected Notification _notification;

		protected const string USERNAME = "username";
		protected const string PASSWORD = "password";
		protected const string COMPUTER_NAME = "someServer";

		[Test]
		public void webdeploy_destination_options_should_have_a_computername()
		{
			Assert.That(_webDeployOptions.ComputerName, Is.EqualTo(COMPUTER_NAME));
		}

		[Test]
		public void webdeploy_destination_options_should_have_a_username()
		{
			Assert.That(_webDeployOptions.UserName, Is.EqualTo(USERNAME));
		}

		[Test]
		public void webdeploy_destination_options_should_have_a_password()
		{
			Assert.That(_webDeployOptions.Password, Is.EqualTo(PASSWORD));
		}

		[Test]
		public void should_have_no_notifications()
		{
			Assert.That(_notification.HasErrors, Is.False);
		}
	}
}