using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteOperation : IOperateRemote
    {
        public abstract void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings);
        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);

    }
}