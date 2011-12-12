using System;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;
using Moq;
using Xunit;

namespace ConDep.WebDeploy.Dsl.Tests.Providers
{
	public class BasicProviderOperation<TBuilder> : ProviderBuilder where TBuilder : IProviderBuilder<TBuilder>
	{
		public const string DIR_PATH = @"C:\temp\test.txt";
		public const string DIR_TARGET_PATH = @"E:\temp\test.txt";

		public BasicProviderOperation(Func<string, TBuilder> providerAction) : base(new System.Collections.Generic.List<Provider>()) 
		{
			Sync(s => s
							.From.LocalHost()
							.UsingProvider(x => providerAction(DIR_PATH)
														.SetRemotePathTo(DIR_TARGET_PATH))
							.To.Server("myWebServer"));
		}

		public override void OnWebDeployMessage(object sender, WebDeployMessegaEventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}

	public abstract class BasicProviderTests<TProvider, TBuilder> : SimpleTestFixture<TBuilder>
		where TProvider : Provider
		where TBuilder : IProviderBuilder<TBuilder>
	{
		//private readonly Func<string, TBuilder> _providerAction;
		private WebDeployDefinition _definition;
		private readonly Func<string, TBuilder> _providerAction;
		private TProvider _provider;
		private IWebDeploy _webDeployer;

		protected BasicProviderTests(WebDeployDefinition definition)
		{
			_definition = definition;
		}

		protected override void Given()
		{
			_webDeployer = new Mock<IWebDeploy>().Object;
		}

		protected override void When()
		{
			var builder = new ProviderBuilder(_definition.Source.Providers);
			var providerOperation = new BasicProviderOperation<TBuilder>(_definition, _webDeployer, _providerAction);
			_provider = (TProvider)_definition.Source.Providers[0];
		}

		[Fact]
		public void should_have_source_path_set()
		{
			var webDeploySourceOptions = new DeploymentBaseOptions();
			var webDeploySource = _provider.GetWebDeploySourceObject(webDeploySourceOptions);

			Assert.Equal(BasicProviderOperation<TBuilder>.DIR_PATH, webDeploySource.AbsolutePath);
			Assert.Equal(BasicProviderOperation<TBuilder>.DIR_PATH, _provider.SourcePath);
		}

		[Fact]
		public void should_have_destination_path_set()
		{
			var webDeployOptions = _provider.GetWebDeployDestinationProviderOptions();

			Assert.Equal("asdf", webDeployOptions.Path);
			Assert.Equal(BasicProviderOperation<TBuilder>.DIR_TARGET_PATH, _provider.DestinationPath);
		}

		[Fact]
		public void should_be_valid()
		{
			Assert.True(_provider.IsValid());
		}
	}
}