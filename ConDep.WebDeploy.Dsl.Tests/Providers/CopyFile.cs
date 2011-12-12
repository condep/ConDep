using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests.Providers
{
	public class when_using_copy_file_provider : ProviderTestFixture<CopyFileProvider>
	{
		protected override void When()
		{
			Providers
				.CopyFile(SourcePath)
				.SetRemotePathTo(DestinationPath);
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
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly().Location;
			}
		}

		public string DestinationPath
		{
			get { return @"E:\tmp"; }
		}
	}
}