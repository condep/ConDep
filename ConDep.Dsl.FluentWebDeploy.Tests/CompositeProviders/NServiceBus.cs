using System;
using ConDep.Dsl.FluentWebDeploy;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests.CompositeProviders
{
    public class NServiceBus : ProviderTestFixture<NServiceBusProvider>
    {
        protected override void When()
        {
            Providers
                .NServiceBus(SourcePath)
                .ToDirectory(DestinationPath);
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