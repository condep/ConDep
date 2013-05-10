using System;

namespace ConDep.Dsl.SemanticModel
{
    public interface IReportStatus
    {
        bool HasErrors { get; }
        bool HasExitCodeErrors { get; }
        void AddUntrappedException(Exception exception);
        void AddConditionMessage(string message);
    }
}