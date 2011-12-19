using System;
using System.Collections.Generic;
using ConDep.Dsl.FluentWebDeploy.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
    public class WebDeployDefinition : IWebDeployModel
	{
		private readonly Source _source = new Source();
		private readonly Destination _destination = new Destination();
		private readonly Configuration _configuration = new Configuration();
		private readonly List<IProvide> _providers = new List<IProvide>();
	    private readonly WebDeploy _webDeploy;

	    public WebDeployDefinition()
        {
            _webDeploy = new WebDeploy(this);
        }

        public Source Source
		{
			get { return _source; }
		}

		public List<IProvide> Providers { get { return _providers; } }

		public Destination Destination
		{
			get { return _destination; }
		}

		public Configuration Configuration
		{
			get { return _configuration; }
		}

		public bool IsValid(Notification notification)
		{
			if (_providers.Count == 0) notification.AddError(new SemanticValidationError("No providers have been specified", ValidationErrorType.NoProviders));

			ValidateChildren(notification);

			return !notification.HasErrors;
		}

		private void ValidateChildren(Notification notification)
		{
			_source.IsValid(notification);
			_destination.IsValid(notification);
			_configuration.IsValid(notification);
			_providers.ForEach(p => p.IsValid(notification));
		}

        public DeploymentStatus Sync(Action<object, WebDeployMessageEventArgs> output, Action<object, WebDeployMessageEventArgs> outputError)
        {
            return _webDeploy.Sync(output, outputError);
        }

	}
}