using System;
using System.Diagnostics;

namespace ConDep.Dsl.Logging
{
    public interface ILogForConDep
    {
        void Warn(string message, params object[] formatArgs);
        void Warn(string message, Exception ex,  params object[] formatArgs);
        void Verbose(string message, params object[] formatArgs);
        void Verbose(string message, Exception ex, params object[] formatArgs);
        void Info(string message, params object[] formatArgs);
        void Info(string message, Exception ex, params object[] formatArgs);
        void Log(string message, TraceLevel traceLevel, params object[] formatArgs);
        void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs);
        void Error(string message, params object[] formatArgs);
        void Error(string message, Exception ex, params object[] formatArgs);
        void LogSectionStart(string name);
        void LogSectionEnd(string name);
        TraceLevel TraceLevel { get; set; }
    }
}