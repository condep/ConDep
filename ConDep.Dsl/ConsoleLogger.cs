using System.Diagnostics;
using log4net;

namespace ConDep.Dsl
{
    public class ConsoleLogger : LoggerBase
    {
        public ConsoleLogger(ILog log) : base(log)
        {
        }

        public override void LogSectionStart(string name)
        {
            Log("TeamCity section start: " + name, TraceLevel.Verbose);
        }

        public override void LogSectionEnd(string name)
        {
            Log("TeamCity section end: " + name, TraceLevel.Verbose);
            
        }
    }
}