using System;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepOperation : IValidate
	{
        public Action<string, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> BeforeExecute;
        public abstract WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        public Action<string, EventHandler<WebDeployMessageEventArgs>, EventHandler<WebDeployMessageEventArgs>, WebDeploymentStatus> AfterExecute;
        public abstract bool IsValid(Notification notification);
    }
}