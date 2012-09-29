using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
	public abstract class ConDepOperationBase : IValidate
	{
        public Action<string, WebDeploymentStatus> BeforeExecute;
        public abstract WebDeploymentStatus Execute(WebDeploymentStatus webDeploymentStatus);
        public Action<string, WebDeploymentStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);

	    public virtual void PrintExecutionSequence(TextWriter writer)
	    {
            writer.WriteLine(GetType().Name);
	    }
	}
}