using System;
using System.Collections.Generic;
using ConDep.Dsl;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
	[TestFixture]
	public abstract class ProviderTestFixture<TProvider, TProvideFor> : SimpleTestFixtureBase 
        where TProvider : class, IProvide
        where TProvideFor : class, IProvideOptions, new()
	{
		private TProvideFor _providers;
		private List<IProvide> _internalProviders;
		private readonly Notification _notification = new Notification();

	    protected TProvideFor Providers
		{
			get
			{
				if (_providers == null)
				{
					_internalProviders = new List<IProvide>();
                    _providers = new TProvideFor();
				    ((IProvideOptions) _providers).AddProviderAction = AddSubProvidersCalled;
				}
				return _providers;
			}
		}

	    private void AddSubProvidersCalled(IProvide obj)
	    {
	        _internalProviders.Add(obj);
	    }

	    protected TProvider Provider
		{
			get { return _internalProviders[0] as TProvider; }
		}

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
            Provider.IsValid(Notification);
            Assert.That(Notification.HasErrors, Is.False);
		}

        [Test]
        public virtual void should_return_webdeploy_destination_object_without_issues()
        {
            var provider = Provider as WebDeployProviderBase;
            if(provider != null)
            {
                provider.GetWebDeployDestinationObject();
            }
        }

        [Test]
        public virtual void should_return_webdeploy_deploy_source_object_without_issues()
        {
            var provider = Provider as WebDeployProviderBase;
            if (provider != null)
            {
                provider.GetWebDeploySourceObject(new Microsoft.Web.Deployment.DeploymentBaseOptions());
            }
        }

	}
}   