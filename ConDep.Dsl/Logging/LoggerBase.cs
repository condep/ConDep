using System;
using System.Diagnostics;

namespace ConDep.Dsl.Logging
{
    public abstract class LoggerBase : ILogForConDep
    {
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

        public virtual void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            Log(message, null, traceLevel, formatArgs);
        }

        public abstract void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs);

        public abstract void LogSectionStart(string name);

        public abstract void LogSectionEnd(string name);

        public abstract TraceLevel TraceLevel { get; set; }
    }
}