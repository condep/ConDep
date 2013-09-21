using System;
using System.Collections.Generic;
using System.IO;
using ConDep.Dsl.Logging;
using NUnit.Framework;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System.Linq;
using Logger = ConDep.Dsl.Logging.Logger;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class TeamCityLoggerTests
    {
        private ILogForConDep _tcServiceMessageLogger;
        private MemoryAppender _tcServiceMessageAppender;
        private const string MESSAGE = "Test";

        [SetUp]
        public void Setup()
        {
            var type = GetType();

            using (
                var logConfigStream =
                    type.Module.Assembly.GetManifestResourceStream(type.Namespace + ".internal.condep.log4net.xml"))
            {
                XmlConfigurator.Configure(logConfigStream);
            }

            _tcServiceMessageLogger = CreateMemoryLogger();
        }

        private ILogForConDep CreateMemoryLogger()
        {
            var memAppender = new MemoryAppender { Name = "MemoryAppender" };

            var repo = LogManager.GetRepository() as Hierarchy;
            var tcMsgAppender = repo.GetAppenders().Single(x => x.Name == "TeamCityServiceMessageAppender") as ConsoleAppender;

            memAppender.Layout = tcMsgAppender.Layout;
            memAppender.ActivateOptions();

            repo.Root.AddAppender(memAppender);
            repo.Configured = true;
            repo.RaiseConfigurationChanged(EventArgs.Empty);
            _tcServiceMessageAppender = memAppender;
            return new TeamCityLogger(LogManager.GetLogger("root"));
        }

        [Test]
        public void TestThatTeamCityBlockOpenedMessageIsProperlyFormatted()
        {
            _tcServiceMessageLogger.LogSectionStart(MESSAGE);
            var returnedMessages = GetMessages().ToList();
            Assert.That(returnedMessages[0], Is.EqualTo("##teamcity[blockOpened name='" + MESSAGE + "']"));
            Assert.That(returnedMessages[1], Is.EqualTo("##teamcity[progressMessage '" + MESSAGE + "']"));
        }

        [Test]
        public void TestThatTeamCityBlockClosedMessageIsProperlyFormatted()
        {
            _tcServiceMessageLogger.LogSectionEnd(MESSAGE);
            var returnedMessage = GetMessages().Single();
            Assert.That(returnedMessage, Is.EqualTo(string.Format("##teamcity[blockClosed name='{0}']", MESSAGE)));
        }

        [Test]
        public void TestThatTeamCityWarnMessageIsProperlyFormatted()
        {
            _tcServiceMessageLogger.Warn(MESSAGE);
            var returnedMessage = GetMessages().Single();
            Assert.That(returnedMessage, Is.EqualTo(string.Format("##teamcity[message text='{0}' errorDetails='' status='{1}']", MESSAGE, TeamCityMessageStatus.WARNING)));
        }

        [Test]
        public void TestThatTeamCityErrorMessageIsProperlyFormatted()
        {
            _tcServiceMessageLogger.Error(MESSAGE, "");
            var returnedMessage = GetMessages().Single();
            Assert.That(returnedMessage, Is.EqualTo(string.Format("##teamcity[message text='{0}' errorDetails='' status='{1}']", MESSAGE, TeamCityMessageStatus.ERROR)));
        }

        public IEnumerable<string> GetMessages()
        {
            var events = _tcServiceMessageAppender.GetEvents();

            foreach (var messageEvent in events)
            {
                using (var memStream = new MemoryStream())
                {
                    var writer = new StreamWriter(memStream);
                    _tcServiceMessageAppender.Layout.Format(writer, messageEvent);
                    writer.Flush();
                    memStream.Position = 0;
                    var reader = new StreamReader(memStream);
                    yield return reader.ReadToEnd();
                }
            }
        }
    }
}