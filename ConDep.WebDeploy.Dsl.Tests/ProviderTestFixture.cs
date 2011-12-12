using System;
using System.Collections.Generic;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests
{
	[TestFixture]
	public abstract class ProviderTestFixture<TProvider> : SimpleTestFixture where TProvider : Provider
	{
		private ProviderCollectionBuilder _providers;
		private List<Provider> _internalProviders;
		private readonly Notification _notification = new Notification();

		protected ProviderCollectionBuilder Providers
		{
			get
			{
				if (_providers == null)
				{
					_internalProviders = new List<Provider>();
					_providers = new ProviderCollectionBuilder(_internalProviders);
				}
				return _providers;
			}
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
			Provider.IsValid(_notification);
		}

		[Test]
		public void should_have_no_notifications()
		{
			Assert.That(Notification.HasErrors, Is.False);
		}

		//[Test]
		//public virtual void should_return_webdeploy_provider_options_without_issues()
		//{
		//   Provider.GetWebDeployDestinationProviderOptions();
		//}

		//[Test]
		//public virtual void should_return_webdeploy_deploy_object_without_issues()
		//{
		//   Provider.GetWebDeploySourceObject(new Microsoft.Web.Deployment.DeploymentBaseOptions());
		//}

	}
}