using System;
using System.Collections.Generic;

namespace ConDep.Server.Execution
{
    public enum ExecutionStatus
    {
        Success,
        Failure,
        Cancelled
    }

    public class ExecutionResult
    {
        private List<Tuple<DateTime, Exception>> _exceptions = new List<Tuple<DateTime, Exception>>();
 
        public ExecutionStatus Status { get; set; }

        public void AddException(DateTime dateTime, Exception ex)
        {
            _exceptions.Add(new Tuple<DateTime, Exception>(dateTime, ex));
        }

        public void AddException(Exception ex)
        {
            _exceptions.Add(new Tuple<DateTime, Exception>(DateTime.UtcNow, ex));
        }

        public IEnumerable<Tuple<DateTime, Exception>> Exceptions { get { return _exceptions; } }

        public bool HasExceptions()
        {
            return _exceptions.Count > 0;
        }
    }


}