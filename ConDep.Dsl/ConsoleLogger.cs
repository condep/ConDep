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
            
        }

        public override void LogSectionEnd(string name)
        {
            
        }
    }
}