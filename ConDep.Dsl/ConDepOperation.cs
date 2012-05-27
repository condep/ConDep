using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl
{
	public abstract class ConDepOperation
	{
	   private readonly Notification _notification;
		private readonly SetupOperation _setupOperation;

		protected ConDepOperation()
		{
			_setupOperation = new SetupOperation();
		    _notification = new Notification();
		}

		protected abstract void OnMessage(object sender, WebDeployMessageEventArgs e);
		protected abstract void OnErrorMessage(object sender, WebDeployMessageEventArgs e);

		protected WebDeploymentStatus Setup(Action<DeploymentOptions> action)
		{
			var status = new WebDeploymentStatus();

			action(new DeploymentOptions(_setupOperation));
			if (!_setupOperation.IsValid(_notification))
			{
				_notification.Throw();
			}

			_setupOperation.Execute(OnMessage, OnErrorMessage, status);
			
			return status;
		}
	}
}