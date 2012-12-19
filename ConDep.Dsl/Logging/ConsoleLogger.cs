using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;

namespace ConDep.Dsl.Logging
{
    public class ConsoleLogger : LoggerBase
    {
        private int _indentLevel;

        public ConsoleLogger(ILog log) : base(log)
        {
        }

        public override void LogSectionStart(string name)
        {
            var sectionName = _indentLevel == 0 ? name : "-- " + name;
            var prefix = GetSectionPrefix();
            base.Log(prefix + sectionName, TraceLevel.Info);
            _indentLevel++;
        }

        public override void LogSectionEnd(string name)
        {
            _indentLevel--;
            var prefix = GetSectionPrefix();
            base.Log(prefix, TraceLevel.Info);
        }

        public override void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            var prefix = GetSectionPrefix();
            //var messages = SplitMessagesByWindowSize(message, prefix);
            //foreach (var messageChunk in messages)
            //{
                base.Log(prefix + " " + message, traceLevel, formatArgs);
            //}
        }

        private IEnumerable<string> SplitMessagesByWindowSize(string message, string prefix)
        {
            var chunkSize = Console.BufferWidth - 18 - prefix.Length;
            if(message.Length <= chunkSize)
            {
                return new [] {message};
            }

            return Enumerable.Range(0, message.Length / chunkSize)
                        .Select(i => message.Substring(i * chunkSize, chunkSize));
        }

        private string GetSectionPrefix()
        {
            const string levelIndicator = "| ";
            var prefix = "";
            for (var i = 0; i < _indentLevel; i++)
            {
                prefix += levelIndicator;
            }
            return prefix;
        }
    }
}