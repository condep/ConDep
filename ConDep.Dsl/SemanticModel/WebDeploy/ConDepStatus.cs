using System;
using System.Collections.Generic;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class ConDepStatus : IReportStatus
    {
        private readonly DateTime _startTime;
        private DateTime _endTime;

        public ConDepStatus()
        {
            _startTime = DateTime.Now;
        }

        //public bool HasErrors
        //{
        //    get
        //    {
        //        return false;// _summeries.Any(s => s.Errors > 0) || _untrappedExceptions.Count > 0;
        //    }
        //}

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public void PrintSummary()
        {
            if (_endTime < _startTime)
            {
                _endTime = DateTime.Now;
            }

            string message = string.Format(@"
Start Time      : {0}
End time        : {1}
Time Taken      : {2}
", StartTime.ToLongTimeString(), EndTime.ToLongTimeString(), (EndTime - StartTime).ToString(@"%h' hrs '%m' min '%s' sec'"));
            Logger.Info("\n");
            Logger.WithLogSection("Summary", () => Logger.Info(message));
        }
    }
}