using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Tests
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