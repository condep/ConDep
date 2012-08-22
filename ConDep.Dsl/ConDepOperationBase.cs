using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepOperationBase : IValidate
	{
        public Action<string, TraceLevel, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> BeforeExecute;
        public abstract WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        public Action<string, TraceLevel, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);
    }
}