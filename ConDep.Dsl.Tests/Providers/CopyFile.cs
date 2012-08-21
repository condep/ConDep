using NUnit.Framework;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Tests.Providers
{
	public class when_using_copy_file_provider : ProviderTestFixture<CopyFileProvider, IProvideForDeployment>
	{
		protected override void When()
		{
			Providers
				.CopyFile(SourceFile, c => c.RenameFileOnDestination(DestinationFileName));
		}

		[Test]
		public void should_have_valid_source_path()
		{
			Assert.That(SourceFile, Is.EqualTo(Provider.SourcePath));
		}

		[Test]
		public void should_have_valid_destination_path()
		{
			Assert.That(DestinationFileName, Is.EqualTo(Provider.DestinationPath));
		}

		public string SourceFile
		{
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly().Location;
			}
		}

		public string DestinationFileName
		{
			get { return @"E:\tmp"; }
		}
	}
}