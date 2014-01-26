using System.Threading;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public interface IExecuteOnServer
    {
        void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings, CancellationToken token);
        string Name { get; }
    }
}