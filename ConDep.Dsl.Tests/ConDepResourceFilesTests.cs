using System.IO;
using System.Reflection;
using ConDep.Dsl.Resources;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class ConDepResourceFilesTests
    {
        [Test]
        public void TestThatFilePathForResourceIsValid()
        {
            var path = ConDepResourceFiles.GetFilePath(Assembly.GetExecutingAssembly(), GetType().Namespace, "ResourceTestFile.txt");
            Assert.That(File.Exists(path));
            File.Delete(path);
        }

        [Test]
        [ExpectedException(typeof(ConDepResourceNotFoundException))]
        public void TestThatUsingFilePathInternalFailsWhenUsingFromExternalAssembly()
        {
            ConDepResourceFiles.GetFilePath(GetType().Assembly, GetType().Namespace, "ResourceTestFile.txt");
        }
    }
}