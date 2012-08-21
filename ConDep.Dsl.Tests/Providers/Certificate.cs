using NUnit.Framework;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Tests.Providers
{
	public class when_using_certificate_provider : ProviderTestFixture<CertficiateDeploymentProvider, IProvideForDeploymentIis>
	{
		protected override void When()
		{
			Providers.Certificate(SourcePath);
		}

		[Test]
		public void should_have_valid_source_path()
		{
			Assert.That(SourcePath, Is.EqualTo(Provider.SourcePath));
		}

		[Test]
		public void should_not_have_destination_path()
		{
			Assert.That(Provider.DestinationPath, Is.Null.Or.Empty);
		}

		public string SourcePath
		{
			get { return "88d927fef15fc7d19ac60810921a3cf46aac8af1"; }
		}
	}
}