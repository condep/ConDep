using System;
using System.Diagnostics;
using System.ServiceProcess;
using ConDep.Dsl.Logging;
using NUnit.Framework;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Logger = ConDep.Dsl.Logging.Logger;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class LoggerTests
    {
        private UnitTestLogger _memLog;

        [SetUp]
        public void Setup()
        {
            _memLog = CreateMemoryLogger();
        }

        private UnitTestLogger CreateMemoryLogger()
        {
            var memAppender = new MemoryAppender {Name = "MemoryAppender"};
            memAppender.ActivateOptions();

            var repo = LogManager.GetRepository() as Hierarchy;
            repo.Root.AddAppender(memAppender);
            repo.Configured = true;
            repo.RaiseConfigurationChanged(EventArgs.Empty);

            return new UnitTestLogger(LogManager.GetLogger("root"), memAppender);
        }

        [Test]
        public void TestThatLoggerResolvesToCorrectLogger()
        {
            var type = GetType();

            using (
                var logConfigStream =
                    type.Module.Assembly.GetManifestResourceStream(type.Namespace + ".internal.condep.log4net.xml"))
            {
                XmlConfigurator.Configure(logConfigStream);
            }

            Assert.That(new Logger().Resolve(), RunningOnTeamCity ? Is.TypeOf<TeamCityLogger>() : Is.TypeOf<ConsoleLogger>());
        }

        [Test]
        public void TestThatTraceLevelOffResultInNoLogging()
        {
            _memLog.TraceLevel = TraceLevel.Off;
            _memLog.Log("Dummy message", TraceLevel.Verbose);
            Assert.That(_memLog.Events.Length, Is.EqualTo(0));
        }

        [Test]
        public void TestThatTraceLevelDoesNotLogLowerThanCurrent()
        {
            ChechNotLower(TraceLevel.Info);
            ChechNotLower(TraceLevel.Warning);
            ChechNotLower(TraceLevel.Error);
        }

        [Test]
        public void TestThatTraceLevelDoesLogConfiguredLevel()
        {
            CheckSpecified(TraceLevel.Info);
            CheckSpecified(TraceLevel.Warning);
            CheckSpecified(TraceLevel.Error);
        }

        [Test]
        public void TestThatTraceLevelDoesLogHigherThanCurrent()
        {
            CheckHigher(TraceLevel.Verbose);
            CheckHigher(TraceLevel.Info);
            CheckHigher(TraceLevel.Warning);
        }

        private void CheckHigher(TraceLevel traceLevel)
        {
            _memLog.TraceLevel = traceLevel;
            _memLog.Log("Dummy message", TraceLevel.Error);
            Assert.That(_memLog.Events.Length, Is.EqualTo(1));
            _memLog.ClearEvents();
        }

        private void CheckSpecified(TraceLevel traceLevel)
        {
            _memLog.TraceLevel = traceLevel;
            _memLog.Log("Dummy message", traceLevel);
            Assert.That(_memLog.Events.Length, Is.EqualTo(1));
            _memLog.ClearEvents();
        }

        private void ChechNotLower(TraceLevel traceLevel)
        {
            _memLog.TraceLevel = traceLevel;
            _memLog.Log("Dummy message", TraceLevel.Verbose);
            Assert.That(_memLog.Events.Length, Is.EqualTo(0));
            _memLog.ClearEvents();
        }

        private bool RunningOnTeamCity
        {
            get
            {
                try
                {
                    var tcService = new ServiceController("TCBuildAgent");
                    return tcService.Status == ServiceControllerStatus.Running;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}