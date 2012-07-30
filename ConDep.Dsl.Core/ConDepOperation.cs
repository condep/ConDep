using System;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepOperation : IValidate
	{
        public Action<string, EventHandler<WebDeployMessageEventArgs>> BeforeExecute;
        public abstract WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        public Action<string, EventHandler<WebDeployMessageEventArgs>> AfterExecute;
        public abstract bool IsValid(Notification notification);
    }
}