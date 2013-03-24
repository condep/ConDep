using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local
{
    public abstract class LocalOperation : IValidate, IExecute
	{
        public abstract void Execute(IReportStatus status, ConDepSettings settings);
        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);
	}
}