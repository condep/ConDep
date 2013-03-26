using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class PSRemotingTests
    {
        [Test]
        public void Test()
        {
            var server = new ServerConfig { Name = "ec2-54-228-126-124.eu-west-1.compute.amazonaws.com", DeploymentUser = new DeploymentUserConfig { UserName = @".\Administrator", Password = "ConDep#1" } };
            //var server = new ServerConfig { Name = "jat-web02", DeploymentUser = new DeploymentUserConfig { UserName = @"torresdal\jat", Password = "GrY,helene#1" } };
            var op = new RemotePowerShellHostOperation(server, "get-module");
            op.Execute(null, null);
        }
         
    }
}