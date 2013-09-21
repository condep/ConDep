using System;
using System.Diagnostics;
using log4net;
using log4net.Core;

namespace ConDep.Dsl.Logging
{
    public abstract class Log4NetLoggerBase : LoggerBase
    {
        protected readonly ILog _log4netLog;

        protected Log4NetLoggerBase(ILog log4netLog)
        {
            _log4netLog = log4netLog;
            ((log4net.Repository.Hierarchy.Logger) _log4netLog.Logger).Level = Level.Info;
        }

        public override TraceLevel TraceLevel
        {
            get
            {
                if (((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level.Name == Level.Debug.Name) return TraceLevel.Verbose;
                if (((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level.Name == Level.Warn.Name) return TraceLevel.Warning;
                if (((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level.Name == Level.Info.Name) return TraceLevel.Info;
                if (((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level.Name == Level.Error.Name) return TraceLevel.Error;
                if (((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level.Name == Level.Off.Name) return TraceLevel.Off;

                return TraceLevel.Verbose;
            }
            set
            {
                switch (value)
                {
                    case TraceLevel.Verbose:
                        ((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level = Level.Debug;
                        break;
                    case TraceLevel.Warning:
                        ((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level = Level.Warn;
                        break;
                    case TraceLevel.Error:
                        ((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level = Level.Error;
                        break;
                    case TraceLevel.Off:
                        ((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level = Level.Off;
                        break;
                    case TraceLevel.Info:
                        ((log4net.Repository.Hierarchy.Logger)_log4netLog.Logger).Level = Level.Info;
                        break;
                }
            }
        }

        public override void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            var level = GetLog4NetLevel(traceLevel);
            var formattedMessage = (formatArgs != null && formatArgs.Length > 0) ? string.Format(message, formatArgs) : message;

            string[] lines = formattedMessage.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var inlineMessage in lines)
            {
                _log4netLog.Logger.Log(typeof(Logger), level, inlineMessage, ex);
            }

        }

        private static Level GetLog4NetLevel(TraceLevel traceLevel)
        {
            switch (traceLevel)
            {
                case TraceLevel.Verbose:
                    return Level.Debug;
                case TraceLevel.Warning:
                    return Level.Warn;
                case TraceLevel.Error:
                    return Level.Error;
                case TraceLevel.Info:
                    return Level.Info;
                case TraceLevel.Off:
                    return Level.Off;
            }
            return Level.Verbose;
        }

    }
}