using System;
using System.Diagnostics;
using log4net;
using log4net.Core;

namespace ConDep.Dsl.Logging
{
    public abstract class LoggerBase : ILogForConDep
    {
        protected readonly ILog _log4netLog;

        protected LoggerBase(ILog log4netLog)
        {
            _log4netLog = log4netLog;
        }

        public virtual void Warn(string message, params object[] formatArgs)
        {
            Log(message, TraceLevel.Warning, formatArgs);
        }

        public virtual void Warn(string message, Exception ex, params object[] formatArgs)
        {
            Log(message, ex, TraceLevel.Warning, formatArgs);
        }

        public virtual void Verbose(string message, params object[] formatArgs)
        {
            Log(message, TraceLevel.Verbose, formatArgs);
        }

        public virtual void Verbose(string message, Exception ex, params object[] formatArgs)
        {
            Log(message, ex, TraceLevel.Verbose, formatArgs);
        }

        public virtual void Info(string message, params object[] formatArgs)
        {
            Log(message, TraceLevel.Info, formatArgs);
        }

        public virtual void Info(string message, Exception ex, params object[] formatArgs)
        {
            Log(message, ex, TraceLevel.Info, formatArgs);
        }

        public virtual void Error(string message, params object[] formatArgs)
        {
            Log(message, TraceLevel.Error, formatArgs);
        }

        public virtual void Error(string message, Exception ex, params object[] formatArgs)
        {
            Log(message, ex, TraceLevel.Error, formatArgs);
        }

        public abstract void LogSectionStart(string name);

        public abstract void LogSectionEnd(string name);

        public TraceLevel TraceLevel
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

        public virtual void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            Log(message, null, traceLevel, formatArgs);
        }

        public virtual void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            var level = GetLog4NetLevel(traceLevel);
            var formattedMessage = (formatArgs != null && formatArgs.Length > 0) ? string.Format(message, formatArgs) : message;
            _log4netLog.Logger.Log(typeof(Logger), level, formattedMessage, ex);
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