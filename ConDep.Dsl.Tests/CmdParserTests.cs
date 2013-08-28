using ConDep.Console;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class CmdParserTests
    {
        [Test]
        public void TestThat_()
        {
            var args = new[] {"myassembly.dll", "dev", "MyApp", "--bypassLB"};
            var parser = new CmdDeployParser(args);
            var options = parser.Parse();

            Assert.That(options.AssemblyName, Is.EqualTo(args[0]));
            Assert.That(options.Environment, Is.EqualTo(args[1]));
            Assert.That(options.Application, Is.EqualTo(args[2]));
            Assert.That(options.BypassLB);
        }

        [Test]
        [ExpectedException(typeof(ConDepCmdParseException))]
        public void TestThat_Exception()
        {
            var args = new[] { "myassembly.dll", "dev" };
            var parser = new CmdDeployParser(args);
            var options = parser.Parse();
        }

    }
}