using System;
using System.Collections.Generic;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests.Providers
{
	public abstract class BasicProviderTest : ProviderTestFixture
	{
		private ProviderCollectionBuilder _providers;
		private List<Provider> _internalProviders;

		private Provider _provider;

		protected void Initialize(Func<string, CopyDirBuilder> function)
		{
			function(SourcePath).SetRemotePathTo(DestinationPath);
			_provider = _internalProviders[0];

			Initialize();
		}

		public abstract string SourcePath { get; }
		public abstract string DestinationPath { get; }

		[Test]
		public void validate_source_and_destination_path()
		{
			Assert.That(SourcePath, Is.EqualTo(_provider.SourcePath));
			Assert.That(DestinationPath, Is.EqualTo(_provider.DestinationPath));
		}

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
	}
}