using System;
using System.IO;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public abstract class ConDepOperationBase : IValidate
	{
        public Action<string, IReportStatus> BeforeExecute;
        public abstract IReportStatus Execute(IReportStatus webDeploymentStatus);
        public Action<string, IReportStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);

	    public virtual void PrintExecutionSequence(TextWriter writer)
	    {
            writer.WriteLine(GetType().Name);
	    }
	}
}