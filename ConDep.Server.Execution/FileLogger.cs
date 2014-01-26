using System;
using System.IO;
using System.Linq;
using ConDep.Dsl.Logging;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace ConDep.Server.Execution
{
    public class FileLogger : Log4NetLoggerBase
    {
        private int _indentLevel;
        private const string LEVEL_INDICATOR = " ";

        public FileLogger(string relativeLogPath, ILog log4netLog)
            : base(log4netLog)
        {
            var hier = log4netLog.Logger.Repository as Hierarchy;
            var fileAppender = (FileAppender)hier.GetAppenders().FirstOrDefault(appender => appender.Name.Equals("TimeFileAppender", StringComparison.InvariantCultureIgnoreCase));

            if (fileAppender == null) return;

            fileAppender.File = relativeLogPath;
            fileAppender.ActivateOptions();
        }

        public override void LogSectionStart(string name)
        {
            var sectionName = _indentLevel == 0 ? name : "" + name;
            base.Log(sectionName, TraceLevel.Info);
            _indentLevel++;
        }

        public override void LogSectionEnd(string name)
        {
            _indentLevel--;
        }

        public override void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            var lines = message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var prefix = GetSectionPrefix();
            foreach (var inlineMessage in lines)
            {
                base.Log(prefix + inlineMessage, ex, traceLevel, formatArgs);
            }

        }

        private string GetSectionPrefix()
        {
            var prefix = "";
            for (var i = 0; i < _indentLevel; i++)
            {
                prefix += LEVEL_INDICATOR;
            }
            return prefix;
        }
    }
}