using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.Dsl.Tests.Providers
{
    public class when_using_copy_dir_provider : ProviderTestFixture<CopyDirProvider>
    {
        protected override void Given()
        {
            _provider = new CopyDirProvider(SourceDir, DestinationPath);
        }

        protected override void When()
        {
        }

        [Test]
        public void should_have_valid_source_path()
        {
            Assert.That(SourceDir, Is.EqualTo(_provider.SourcePath));
        }

        [Test]
        public void should_have_valid_destination_path()
        {
            Assert.That(DestinationPath, Is.EqualTo(_provider.DestinationPath));
        }

        public string SourceDir
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }

        public string DestinationPath
        {
            get { return @"E:\tmp"; }
        }
    }
}

