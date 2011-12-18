using System;
using ConDep.Dsl.FluentWebDeploy.Builders;
using ConDep.Dsl.FluentWebDeploy.Deployment;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy
{
	public abstract class WebDeployOperation
	{
		private readonly WebDeployDefinition _definition;
		private readonly IWebDeploy _webDeployer;
	    private Notification _notification;

	    protected WebDeployOperation()
		{
			_definition = new WebDeployDefinition();
			_webDeployer = new WebDeploy();
		    _notification = new Notification();
		}

		public DeploymentStatus Sync(Action<SyncBuilder> action)
		{
			action(new SyncBuilder(_definition));
            if (!_definition.IsValid(_notification))
            {
                _notification.Throw();
            }
			return _webDeployer.Deploy(_definition, OnWebDeployMessage, OnWebDeployErrorMessage);
		}

		protected abstract void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e);
		protected abstract void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e);
	}
}