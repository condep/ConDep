using System;
using System.Collections.Generic;
using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Local.TransformConfig;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel.WebDeploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class ConfigTransformTests
    {
        private ConDepSettings _settingsDefault;

        [SetUp]
        public void Setup()
        {
            FilesToDeleteAfterTest = new List<string>();
            _settingsDefault = new ConDepSettings
            {
                Options =
                {
                    WebDeployExist = true,
                    SuspendMode = LoadBalancerSuspendMethod.Graceful
                }
            };
        }
        
        [TearDown]
        public void CleanUpFilesToDeleteList()
        {
            if (FilesToDeleteAfterTest != null && FilesToDeleteAfterTest.Count > 0)
            {
                foreach (var filename in FilesToDeleteAfterTest)
                {
                    if (File.Exists(filename))
                    {
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (IOException)
                        {
                            // some processes will hold onto the file until the AppDomain is unloaded
                        }
                    }
                }
            }

            FilesToDeleteAfterTest = null;
        }

        [Test]
        public void TestThatTransformConfigOperationCorrectlyTransformsConfigFile()
        {
            var source = WriteTextToTempFile(Consts.Source01);
            var transform = WriteTextToTempFile(Consts.Transform01);
            var destination = source;
            var expectedResultFile = WriteTextToTempFile(Consts.Result01);

            var trans = new TransformConfigOperation(Path.GetDirectoryName(source), Path.GetFileName(source), Path.GetFileName(transform));
            var webDepStatus = new ConDepStatus();
            trans.Execute(webDepStatus, _settingsDefault);

            Assert.That(webDepStatus.HasErrors, Is.False);

            var actualResult = File.ReadAllText(destination);
            var expectedResult = File.ReadAllText(expectedResultFile);
            Assert.AreEqual(expectedResult.Trim(), actualResult.Trim());
        }

        private static class Consts
        {
            public const string Source01 =
@"<?xml version=""1.0""?>
<configuration>
  <appSettings>
    <add key=""setting01"" value=""default01""/> 
    <add key=""setting02"" value=""default02""/> 
  </appSettings>
</configuration>";

            public const string Transform01 =
@"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <appSettings>
    <add key=""setting01"" value=""debug01""
         xdt:Locator=""Match(key)"" xdt:Transform=""Replace"" />
    
    <add key=""setting02"" value=""debug02""
         xdt:Locator=""Match(key)"" xdt:Transform=""Replace"" />
  </appSettings>

</configuration>";

            public const string Result01 =
@"<?xml version=""1.0""?>
<configuration>
  <appSettings>
    <add key=""setting01"" value=""debug01""/> 
    <add key=""setting02"" value=""debug02""/> 
  </appSettings>
</configuration>";
        }

        protected virtual string WriteTextToTempFile(string content)
        {
            if (string.IsNullOrEmpty(content)) { throw new ArgumentNullException("content"); }

            string tempFile = this.GetTempFilename(true);
            File.WriteAllText(tempFile, content);
            return tempFile;
        }

        protected virtual string GetTempFilename(bool ensureFileDoesntExist)
        {
            var path = Path.GetTempFileName();
            if (ensureFileDoesntExist && File.Exists(path))
            {
                File.Delete(path);
            }
            FilesToDeleteAfterTest.Add(path);
            return path;
        }

        protected IList<string> FilesToDeleteAfterTest { get; set; }

    }
}