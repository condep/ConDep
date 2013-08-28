using System;
using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Local.PreCompile;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using Moq;
using NUnit.Framework;

namespace ConDep.Dsl.Tests.Operations.Local
{
    [TestFixture]
    public class PreCompileTests
    {
        private Mock<IWrapClientBuildManager> _buildManager;
        private string _validWebAppPath;
        private string _validOutputPath;
        private ConDepSettings _settingsDefault;

        [SetUp]
        public void Setup()
        {
            _buildManager = new Mock<IWrapClientBuildManager>();
            _buildManager.Setup(x => x.PrecompileApplication(It.IsAny<PreCompileCallback>()));
            _validWebAppPath = Path.GetTempPath();
            _validOutputPath = Environment.CurrentDirectory;
            _settingsDefault = new ConDepSettings
            {
                Options =
                {
                    SuspendMode = LoadBalancerSuspendMethod.Graceful
                }
            };
        }

        [Test]
        public void TestThatPreCompileExecutesSuccessfully()
        {
            var operation = new PreCompileOperation("MyWebApp", @"C:\temp\MyWebApp", @"C:\temp\MyWebAppCompiled", _buildManager.Object);
            
            var status = new StatusReporter();
            operation.Execute(status, _settingsDefault);

            Assert.That(status.HasErrors, Is.False);
            _buildManager.Verify(manager => manager.PrecompileApplication(It.IsAny<PreCompileCallback>()));
        }

        [Test]
        public void TestThatValidationsFailsWhenAppNameIsEmpty()
        {
            var operation = new PreCompileOperation("", _validWebAppPath, _validOutputPath, _buildManager.Object);
            var notification = new Notification();
            Assert.That(operation.IsValid(notification), Is.False);
        }

        [Test]
        public void TestThatValidationsSucceedsWhenDirectoriesExists()
        {
            var operation = new PreCompileOperation("MyWebApp", _validWebAppPath, _validOutputPath, _buildManager.Object);
            var notification = new Notification();
            Assert.That(operation.IsValid(notification));
        }
    }
}