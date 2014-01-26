using System;
using System.Collections.Generic;

namespace ConDep.Dsl.SemanticModel
{
    [Serializable]
    public class ConDepExecutionResult
    {
        private readonly bool _success;
        private readonly List<TimedException> _exceptions = new List<TimedException>();

        public ConDepExecutionResult(bool success)
        {
            _success = success;
        }

        public bool Success
        {
            get { return _success; }
        }

        public bool Cancelled { get; set; }

        public List<TimedException> ExceptionMessages
        {
            get { return _exceptions; }
        }

        public void AddException(Exception exception)
        {
            _exceptions.Add(new TimedException { DateTime = DateTime.UtcNow, Exception = exception });
        }

        public bool HasExceptions()
        {
            return _exceptions.Count > 0;
        }
    }

    [Serializable]
    public class TimedException
    {
        public DateTime DateTime { get; set; }
        public Exception Exception { get; set; }
    }
}