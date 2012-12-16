using System.Diagnostics;
using log4net;

namespace ConDep.Dsl.Logging
{
    public class ConsoleLogger : LoggerBase
    {
        private int indentLevel = 0;

        public ConsoleLogger(ILog log) : base(log)
        {
        }

        public override void LogSectionStart(string name)
        {
            Log("==" + name + "==", TraceLevel.Info);
            indentLevel++;
        }

        public override void LogSectionEnd(string name)
        {
            indentLevel--;
            Log("==" + name + "==", TraceLevel.Info);
        }

        public override void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            base.Log(new string(' ', indentLevel) +  message, traceLevel, formatArgs);
        }
    }
}