using System;
using System.Diagnostics;

namespace ConDep.Server.Api.Controllers
{
    public class ExecutionLogItem
    {
        public string Text { get; set; }

        public TraceLevel LogLevel { get; set; }

        public DateTime UtcTime { get; set; }

        public int IndentLevel { get; set; }

        public string SubLogName { get; set; }
    }
}