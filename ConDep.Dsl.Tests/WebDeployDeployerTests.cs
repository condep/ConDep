using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel.WebDeploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class WebDeployDeployerTests
    {
        [Test]
        [Ignore]
        public void Test()
        {
            var user = new DeploymentUserConfig();

            var server = new ServerConfig() {Name = "jat-web03", DeploymentUser = user };
            using(new WebDeployDeployer(server))
            {
                
            }
        }

    }
}