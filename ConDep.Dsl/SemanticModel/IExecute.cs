using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public interface IExecute
    {
        void Execute(IReportStatus status, ConDepSettings settings);
        string Name { get; }
        void DryRun();
    }
}