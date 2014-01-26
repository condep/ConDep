using System.Threading;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public interface IExecute
    {
        void Execute(IReportStatus status, ConDepSettings settings, CancellationToken token);
        string Name { get; }
        void DryRun();
    }
}