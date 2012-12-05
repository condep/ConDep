using System.Collections.Generic;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
	[TestFixture]
	public abstract class ProviderTestFixture<TProvider> : SimpleTestFixtureBase 
        where TProvider : class, IProvide
	{
		private readonly Notification _notification = new Notification();
	    protected TProvider _provider;

	    protected Notification Notification
		{
			get { return _notification; }
		}

	    protected override void Given()
		{
        }

		protected override void After()
		{
		}

		[Test]
		public void should_have_no_notifications()
		{
            _provider.IsValid(Notification);
            Assert.That(Notification.HasErrors, Is.False);
		}

        [Test]
        public virtual void should_return_webdeploy_destination_object_without_issues()
        {
            var provider = _provider as WebDeployProviderBase;
            if(provider != null)
            {
                provider.GetWebDeployDestinationObject();
            }
        }

        [Test]
        public virtual void should_return_webdeploy_deploy_source_object_without_issues()
        {
            var provider = _provider as WebDeployProviderBase;
            if (provider != null)
            {
                provider.GetWebDeploySourceObject(new Microsoft.Web.Deployment.DeploymentBaseOptions());
            }
        }

	}
}   