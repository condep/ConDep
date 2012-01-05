using System;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Builders
{
	public interface IOperateConDep : IValidate
	{
		WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
	}
}