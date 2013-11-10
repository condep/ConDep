using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public interface IExecuteOnServer
    {
        void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings);
        string Name { get; }
    }
}