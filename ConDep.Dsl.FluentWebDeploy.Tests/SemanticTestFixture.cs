using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Tests
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
}