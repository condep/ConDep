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
            var sectionName = _indentLevel == 0 ? name : "" + name + ":";
            base.Log(sectionName, TraceLevel.Info);
            _indentLevel++;
        }

        public override void LogSectionEnd(string name)
        {
            _indentLevel--;
            //Log("}", TraceLevel.Info);
        }

        public override void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            //var prefix = GetSectionPrefix();
            Log(message, null, traceLevel, formatArgs);
        }

        public override void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            string[] lines = message.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            var prefix = GetSectionPrefix();
            foreach (var inlineMessage in lines)
            {
                var splitMessages = SplitMessagesByWindowSize(inlineMessage, prefix);
                foreach(var splitMessage in splitMessages)
                {
                    base.Log(prefix + splitMessage, ex, traceLevel, formatArgs);
                }
            }

        }

        private IEnumerable<string> SplitMessagesByWindowSize(string message, string prefix)
        {
            var chunkSize = Console.BufferWidth - 18 - prefix.Length;
            if(message.Length <= chunkSize)
            {
                return new [] {message};
            }

            return Chunk(message, chunkSize);
            //return Enumerable.Range(0, message.Length / chunkSize).Select(i => message.Substring(i * chunkSize, chunkSize));
        }

        static IEnumerable<string> Chunk(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                if(i+chunkSize > str.Length)
                {
                    yield return str.Substring(i);
                }
                else
                {
                    yield return str.Substring(i, chunkSize);
                }
            }
        }

        private string GetSectionPrefix()
        {
            const string levelIndicator = "   ";
            var prefix = "";
            for (var i = 0; i < _indentLevel; i++)
            {
                prefix += levelIndicator;
            }
            return prefix;
        }
    }
}