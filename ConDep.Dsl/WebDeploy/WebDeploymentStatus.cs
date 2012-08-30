using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
    public class WebDeploymentStatus
    {
        private readonly List<DeploymentChangeSummary> _summeries = new List<DeploymentChangeSummary>();
        private readonly List<Exception> _untrappedExceptions = new List<Exception>();
        private readonly List<string> _conditionMessages = new List<string>();

        public void AddSummery(DeploymentChangeSummary summery)
        {
            _summeries.Add(summery);
        }

        public bool HasErrors
        {
            get
            {
                return _summeries.Any(s => s.Errors > 0) || _untrappedExceptions.Count > 0;
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