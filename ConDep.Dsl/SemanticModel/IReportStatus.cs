using System;

namespace ConDep.Dsl.SemanticModel
{
    public interface IReportStatus
    {
        bool HasErrors { get; }
    }
}