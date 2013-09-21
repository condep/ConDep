using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConDep.Server.Api.Controllers
{
    public class ExecutionLog
    {
        private readonly string _execId;
        private List<ExecutionLogItem> _log = new List<ExecutionLogItem>();

        public ExecutionLog(string execId)
        {
            _execId = execId;
        }

        public string ExecId
        {
            get { return _execId; }
        }

        public void Log(string message, TraceLevel logLevel)
        {
            var item = new ExecutionLogItem
                {
                    Text = message,
                    LogLevel = logLevel,
                    UtcTime = DateTime.UtcNow,
                    IndentLevel = IndentLevel,
                    SubLogName = SubLogPath
                };
            _log.Add(item);
        }

        public void StartSubLog(string name)
        {
            IndentLevel++;
            SubLogPath += "/" + name;
        }

        protected int IndentLevel { get; set; }
        public void EndSubLog(string name)
        {
            SubLogPath = SubLogPath.TrimEnd(("/" + name).ToCharArray());
            IndentLevel--;
        }

        protected string SubLogPath { get; set; }


        public List<ExecutionLogItem> InternalLog { get { return _log; } } 
    }
}