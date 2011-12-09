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

		private const string SOURCE_PATH = @"C:\tmp";
		private const string DESTINATION_PATH = @"E:\tmp";

		private bool _isInitialized;
		private Provider _provider;

		protected void Initialize(Func<string, CopyDirBuilder> function)
		{
			_isInitialized = true;

			function(SOURCE_PATH).SetRemotePathTo(DESTINATION_PATH);
			_provider = _internalProviders[0];

			base.Initialize();
		}

		[Test]
		public void validate_source_and_destination_path()
		{
			Assert.That(SOURCE_PATH, Is.EqualTo(_provider.SourcePath));
			Assert.That(DESTINATION_PATH, Is.EqualTo(_provider.DestinationPath));
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