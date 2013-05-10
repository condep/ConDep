using System;
using System.Collections.Generic;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeploymentStatus : IReportStatus
    {
        private readonly DateTime _startTime;
        private readonly List<Exception> _untrappedExceptions = new List<Exception>();
        private readonly List<string> _conditionMessages = new List<string>();
        private DateTime _endTime;

        public WebDeploymentStatus()
        {
            _startTime = DateTime.Now;
        }

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

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public void AddConditionMessage(string message)
        {
            _conditionMessages.Add(message);
        }


        public void PrintSummary()
        {
            int objectsAdded = 0;
            int objectsDeleted = 0;
            int objectsUpdated = 0;
            double mBytesCopied = 0;

            //foreach (var summery in _summeries)
            //{
            //    objectsAdded += summery.ObjectsAdded;
            //    objectsDeleted += summery.ObjectsDeleted;
            //    objectsUpdated += summery.ObjectsUpdated;
            //    if(summery.BytesCopied > 0)
            //    {
            //        mBytesCopied += ((summery.BytesCopied / 1024.0) / 1024.0);
            //    }
            //}

            string message = string.Format(@"Objects Added     : {0}
Objects Deleted   : {1}
Objects Updated   : {2}
Mega Bytes Copied : {3}
Time taken        : {4}
", objectsAdded, objectsDeleted, objectsUpdated, mBytesCopied.ToString("N"), (EndTime - StartTime).ToString(@"hh\:mm\:ss"));

            Logger.LogSectionStart("Summary");
            Logger.Info(message);
            Logger.LogSectionEnd("Summary");
        }
    }
}