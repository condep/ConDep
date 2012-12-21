using System;
using ConDep.Dsl.Logging;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace ConDep.Dsl.Tests
{
    class UnitTestLogger : LoggerBase
    {
        private readonly MemoryAppender _memAppender;

        public UnitTestLogger(ILog log4netLog, MemoryAppender memAppender) : base(log4netLog)
        {
            _memAppender = memAppender;
        }

        public override void Warn(string message, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Warn(string message, Exception ex, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Verbose(string message, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Verbose(string message, Exception ex, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Info(string message, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Info(string message, Exception ex, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Error(string message, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void Error(string message, Exception ex, object[] formatArgs)
        {
            throw new NotImplementedException();
        }

        public override void LogSectionStart(string name)
        {
            throw new NotImplementedException();
        }

        public override void LogSectionEnd(string name)
        {
            throw new NotImplementedException();
        }

        public LoggingEvent[] Events { get { return _memAppender.GetEvents(); } }

        public void ClearEvents()
        {
            _memAppender.Clear();
        }
    }
}