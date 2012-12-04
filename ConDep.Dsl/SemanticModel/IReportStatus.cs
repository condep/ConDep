using System;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel
{
    public interface IReportStatus
    {
        void AddSummery(DeploymentChangeSummary summery);
        bool HasErrors { get; }
        bool HasExitCodeErrors { get; }
        void AddUntrappedException(Exception exception);
        void AddConditionMessage(string message);
    }
}