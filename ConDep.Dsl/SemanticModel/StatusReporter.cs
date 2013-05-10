using System;
using System.Collections.Generic;

namespace ConDep.Dsl.SemanticModel
{
    public class StatusReporter : IReportStatus
    {
        private readonly List<Exception> _untrappedExceptions = new List<Exception>();
        private readonly List<string> _conditionMessages = new List<string>();

        public bool HasErrors
        {
            get
            {
                return false;// _summeries.Any(s => s.Errors > 0) || _untrappedExceptions.Count > 0;
            }
        }

        public void AddUntrappedException(Exception exception)
        {
            _untrappedExceptions.Add(exception);
        }

        public bool HasExitCodeErrors
        {
            get { return _untrappedExceptions.Count > 0; }
        }

        public void AddConditionMessage(string message)
        {
            _conditionMessages.Add(message);
        }


    }
}