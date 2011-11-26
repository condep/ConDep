using System.Security.AccessControl;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl;
using ConDep.WebDeploy.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.WebDeploy.Dsl.Tests
{
	[TestFixture]
	public class WebDeploymentTests
	{
		private SyncBuilder _sync;
		private WebDeployDefinition _webDeployDefinition;
		private WebDeploy _webDeploy;

		[SetUp]
		public void Init()
		{
			_webDeployDefinition = new WebDeployDefinition();
			_sync = new SyncBuilder(_webDeployDefinition);
			_webDeploy = new WebDeploy(_webDeployDefinition);
		}

		[Test]
		public void TestThat_WebApp_Works_With_Default_Credentials()
		{
			_sync
				.FromServer("ffdevweb01")
				.UsingProvider(p => p.WebApp(@"agent.frende.no/STS")
				                    	.AddToRemoteWebsite("Default Web Site")
				                    	.SetRemoteAppNameTo("STSSync"))
				.ToLocalHost();

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_WebApp_Works_With_Destination_Credentials()
		{
			_sync
				.FromServer("ffdevweb01")
				.UsingProvider(p => p.WebApp(@"agent.frende.no/STS")
				                    	.AddToRemoteWebsite("Default Web Site")
				                    	.SetRemoteAppNameTo("STSSync")
				)
				.ToLocalHost(c =>
				             	{
				             		c.WithUserName(@"sting\torjon");
				             		c.WithPassword("GrY,helene55");
				             	});

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_WebApp_Works_With_Source_And_Destination_Credentials()
		{
			_sync
				.FromServer("ffdevweb01", c =>
				                          	{
				                          		c.WithUserName(@"sting\torjon");
				                          		c.WithPassword("GrY,helene55");
				                          	})
				.UsingProvider(p => p.WebApp(@"agent.frende.no/STS")
				                    	.AddToRemoteWebsite("Default Web Site")
				                    	.SetRemoteAppNameTo("STSSync")
				)
				.ToLocalHost(c =>
				             	{
				             		c.WithUserName(@"sting\torjon");
				             		c.WithPassword("GrY,helene55");
				             	});

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_CopyDir_Works()
		{
			_sync
				.FromLocalHost()
				.UsingProvider(p => p.CopyDir(@"C:\Temp\Acos")
				                    	.SetRemotePathTo(@"C:\Temp\Acos2"))
				.ToLocalHost();

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_Certificate_Works()
		{
			_sync
				.FromServer("ffdevweb01")
				.UsingProvider(p => p.Certificate(@"my\721e7a242c1ddbf382ab52ded795d32b6de65089"))
				.ToLocalHost();

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_SetAcl_Works()
		{
_sync
	.FromLocalHost()
	.UsingProvider(p =>
				         {
				            p.WebApp(@"agent.frende.no/STS")
				               .AddToRemoteWebsite("Default Web Site")
				               .SetRemoteAppNameTo("STSSync");

				            p.SetAcl(@"C:\Temp\Acos")
				               .Permissions(FileSystemRights.Modify | FileSystemRights.TakeOwnership)
				               .User(@"sting\webdeployer");
				         })
	.ToServer("ffdevweb03", c =>
				                  {
				                     c.WithUserName(@"sting\torjon");
				                     c.WithPassword("GrY,helene55");
				                  });

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_AutoDeployAgent_Works()
		{
			_sync
				.WithConfiguration(c =>
				{
					c.AutoDeployAgent();
				})
				.FromLocalHost()
				.UsingProvider(p => p.CopyDir(@"C:\Temp\Acos")
											.SetRemotePathTo(@"C:\abc\def"))
				.ToServer("ffdevweb03");

			_webDeploy.Deploy();
		}

		[Test]
		public void TestThat_CopyFile_Works()
		{
			_sync
				.WithConfiguration(c =>
				{
					c.AutoDeployAgent();
				})
				.FromLocalHost()
				.UsingProvider(p => p.CopyFile(@"C:\Temp\Acos\ClaimService.cs")
											.SetRemotePathTo(@"C:\abcd\ClaimService.cs"))
				.ToServer("ffdevweb03");

			_webDeploy.Deploy();
		}
	}
}

