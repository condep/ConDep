using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Builders
{
    public interface IExecuteSequence
    {
        IReportStatus Execute(IReportStatus status);
        void Add<T>(T t);
    }
}