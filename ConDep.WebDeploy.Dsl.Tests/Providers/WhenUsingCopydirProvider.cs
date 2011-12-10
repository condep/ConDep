namespace ConDep.WebDeploy.Dsl.Tests.Providers
{
	public class WhenUsingCopyDirProvider : BasicProviderTest
	{
		public WhenUsingCopyDirProvider()
		{
			Initialize(Providers.CopyDir);
		}

		protected override void Given()
		{
		}

		protected override void When()
		{
		}

		public override string SourcePath
		{
			get { return @"C:\tmp"; }
		}

		public override string DestinationPath
		{
			get { return @"E:\tmp"; }
		}
	}
}

