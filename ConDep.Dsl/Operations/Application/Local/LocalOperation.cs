using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local
{
    public abstract class LocalOperation : IValidate, IExecute
	{
        public abstract void Execute(IReportStatus status, ConDepSettings settings, CancellationToken token);
        public abstract string Name { get; }
        public void DryRun()
        {
            Logger.Info(Name);
        }

        public abstract bool IsValid(Notification notification);
	}
}