using NUnit.Framework;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Tests.Providers
{
	public class when_using_copy_dir_provider : ProviderTestFixture<CopyDirProvider, ProvideForDeployment>
	{
		protected override void When()
		{
			Providers
				.CopyDir(SourceDir, c => c.DestinationDir(DestinationPath));
		}

		[Test]
		public void should_have_valid_source_path()
		{
			Assert.That(SourceDir, Is.EqualTo(Provider.SourcePath));
		}

		[Test]
		public void should_have_valid_destination_path()
		{
			Assert.That(DestinationPath, Is.EqualTo(Provider.DestinationPath));
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

