using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface ISetupConDep
    {
        bool IsValid(Notification notification);
        IReportStatus Execute(ConDepOptions options, IReportStatus status);
        void PrintExecutionSequence(ConDepOptions options);
        void AddOperation(LocalOperation operation);
    }
}