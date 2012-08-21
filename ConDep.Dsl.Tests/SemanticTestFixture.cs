using ConDep.Dsl.Core;

namespace ConDep.Dsl.Tests
{
	public abstract class SemanticTestFixture : SimpleTestFixture
	{
		private WebDeployServerDefinition _serverDefinition;

		protected override void Given()
		{
			_serverDefinition = new WebDeployServerDefinition();
			Notification = new Notification();
		}

		protected override void When()
		{
			_serverDefinition.IsValid(Notification);
		}

		protected Notification Notification { get; private set; }
	}
}