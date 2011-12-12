using System.Threading;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests.Providers
{
	public class when_using_copy_dir_provider : ProviderTestFixture<CopyDirProvider>
	{
		protected override void When()
		{
			Providers
				.CopyDir(SourcePath)
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
				return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
		}

		public string DestinationPath
		{
			get { return @"E:\tmp"; }
		}
	}
}

