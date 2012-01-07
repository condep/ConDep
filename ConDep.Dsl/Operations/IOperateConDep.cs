using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
{
	public interface IOperateConDep : IValidate
	{
		WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
	}
}