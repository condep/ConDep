using NUnit.Framework;

namespace ConDep.Dsl.Tests.CompositeProviders
{
    public class when_using_NServiceBus_provider : ProviderTestFixture<NServiceBusProvider>
    {
        protected override void When()
        {
        	Providers
        		.NServiceBus(SourcePath, "serviceName",
        		             config => config
        		                       	.DestinationDir(DestinationPath)
										.ServiceInstaller("asdf")
										.UserName("asdf")
										.Password("asdf")
										.ServiceGroup("asdf"));
        }

        protected string DestinationPath
        {
            get { return "asdf"; }
        }

        [Test]
        public void should_have_valid_source_path()
        {
            Assert.That(SourcePath, Is.EqualTo(Provider.SourcePath));
        }

        [Test]
        public void should_have_valid_destination_path()
        {
            Assert.That(DestinationPath, Is.EqualTo(Provider.DestinationPath));
        }

        public string SourcePath
        {
            get { return "someDirectoryContainingNServiceBusProject"; }
        }

    }
}