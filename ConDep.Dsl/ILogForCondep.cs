using System.Diagnostics;

namespace ConDep.Dsl
{
    public interface ILogForConDep
    {
        void Warn(string message, params object[] formatArgs);
        void Verbose(string message, params object[] formatArgs);
        void Info(string message, params object[] formatArgs);
        void Log(string message, TraceLevel traceLevel, params object[] formatArgs);
        void Error(string message, params object[] formatArgs);
        void Error(string message, string errorDetails, params object[] formatArgs);
        void LogSectionStart(string name);
        void LogSectionEnd(string name);
        TraceLevel TraceLevel { get; set; }
    }
}