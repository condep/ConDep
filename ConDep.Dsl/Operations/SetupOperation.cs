using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations
{
	public class SetupOperation : IOperateConDep
	{
		private readonly List<IOperateConDep> _operations = new List<IOperateConDep>();

		public void AddOperation(IOperateConDep operation)
		{
			_operations.Add(operation);
		}

		public bool IsValid(Notification notification)
		{
			return _operations.All(operation => operation.IsValid(notification));
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			foreach(var operation in _operations)
			{
				operation.Execute(output, outputError, webDeploymentStatus);
			}
			return webDeploymentStatus;
		}
	}
}