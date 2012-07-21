using System;

namespace ConDep.Dsl.Core
{
	public interface IOperateConDep : IValidate
	{
        WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
	}
}