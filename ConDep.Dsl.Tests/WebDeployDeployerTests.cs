using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel.WebDeploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class WebDeployDeployerTests
    {
        [Test]
        public void Test()
        {
            var server = new ServerConfig() {Name = "jat-web03"};
            using(new WebDeployDeployer(server))
            {
                
            }
        }
         
    }
}