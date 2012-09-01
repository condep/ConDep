using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
	public abstract class ConDepOperationBase : IValidate
	{
        public Action<string, TraceLevel, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> BeforeExecute;
        public abstract WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        public Action<string, TraceLevel, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);

	    public virtual void PrintExecutionSequence(TextWriter writer)
	    {
            writer.WriteLine(GetType().Name);
	    }
	}
}