using System;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local
{
    public abstract class LocalOperation : IValidate
	{
        public Action<string, IReportStatus> BeforeExecute;
        public abstract IReportStatus Execute(IReportStatus webDeploymentStatus);
        public Action<string, IReportStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);
	}
}