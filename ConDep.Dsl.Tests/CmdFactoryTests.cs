using ConDep.Console;
using ConDep.Console.Deploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class CmdFactoryTests
    {
        [Test]
        public void TestThat_FactoryCanResolveDeploymentCommand()
        {
            var factory = new CmdFactory(new[] { "deploy" });
            Assert.That(factory.Resolve(), Is.InstanceOf<CmdDeployHandler>());
        }
         
    }
}