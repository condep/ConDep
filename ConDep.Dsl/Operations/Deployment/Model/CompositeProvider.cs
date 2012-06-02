using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Operations.WebDeploy.Model
{
	public abstract class CompositeProvider : IProvide
	{
		private readonly List<IProvide> _childProviders = new List<IProvide>();

		public List<IProvide> ChildProviders { get { return _childProviders; } }

		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }

		public abstract bool IsValid(Notification notification);

		public int WaitInterval { get; set; }
        public int RetryAttempts { get; set; }

		public abstract void Configure();

		protected void Configure(Action<ProviderOptions> action)
		{
			action(new ProviderOptions(_childProviders));
		}

        public WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
			  if (WaitInterval > 0)
			  {
				  webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
			  }

              if (RetryAttempts > 0)
                  webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;


            ChildProviders.Reverse();
            foreach (var childProvider in ChildProviders)
            {
                childProvider.Sync(webDeployOptions, deploymentStatus);
            }
            return deploymentStatus;
        }
	}
}