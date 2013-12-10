using System;
using System.Collections.Generic;
using System.Reflection;
using ConDep.Dsl.Config;

namespace ConDep.Server.Api.Model
{
    public class ExecutionStatus
    {
        private List<ExecutionEvent> _events = new List<ExecutionEvent>();

        public string ExecId { get; set; }
        public string Environment { get; set; }
        public string Module { get; set; }
        public List<ExecutionEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }
        public IEnumerable<Exception> Exceptions { get; set; }
        public DateTime StartedUtc { get; set; }
        public DateTime FinishedUtc { get; set; }
        public string RelativeLogLocation { get; set; }
        public FinishedStatus FinishedStatus { get; set; }
    }

    public enum FinishedStatus
    {
        NotFinished,
        Success,
        Failed
    }

    public class ExecutionEvent
    {
        public DateTime DateUtc { get; set; }
        public string Message { get; set; }
    }
}