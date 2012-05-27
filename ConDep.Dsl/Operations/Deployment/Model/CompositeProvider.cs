using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Operations.WebDeploy.Model
{
	public abstract class CompositeProvider : IProvide
	{
		private readonly List<IProvide> _childProviders = new List<IProvide>();

		public IEnumerable<IProvide> ChildProviders { get { return _childProviders; } }

		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }

		public abstract bool IsValid(Notification notification);

		public int WaitInterval { get; set; }

		public abstract void Configure();

		protected void Configure(Action<ProviderCollection> action)
		{
			action(new ProviderCollection(_childProviders));
		}

        public WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
			  if (WaitInterval > 0)
			  {
				  webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
			  }

            foreach (var childProvider in ChildProviders)
            {
                childProvider.Sync(webDeployOptions, deploymentStatus);
            }
            return deploymentStatus;
        }
	}
}