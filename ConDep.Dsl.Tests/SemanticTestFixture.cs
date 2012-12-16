using System;
using ConDep.Dsl;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Tests
{
	public abstract class SemanticTestFixture : SimpleTestFixtureBase
	{
        //private WebDeployServerDefinition _serverDefinition;

		protected override void Given()
		{
            throw new NotImplementedException();
            //_serverDefinition = new WebDeployServerDefinition();
            //Notification = new Notification();
		}

		protected override void When()
		{
            throw new NotImplementedException();
            //_serverDefinition.IsValid(Notification);
		}

		protected Notification Notification { get; private set; }
	}
}