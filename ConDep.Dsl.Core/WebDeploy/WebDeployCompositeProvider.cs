using System;
using System.Linq;
using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
	public abstract class WebDeployCompositeProvider : IProvide
	{
	    private readonly List<IProvide> _childProviders = new List<IProvide>();
	    private readonly List<WebDeployExecuteCondition> _conditions = new List<WebDeployExecuteCondition>();

	    public List<IProvide> ChildProviders { get { return _childProviders; } }
        public IEnumerable<WebDeployExecuteCondition> ExecuteConditions { get { return _conditions; } }
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }

		public abstract bool IsValid(Notification notification);

		public int WaitInterval { get; set; }
        public int RetryAttempts { get; set; }

        public abstract void Configure(DeploymentServer server);

        protected void Configure(Action<ProviderOptions> action)
		{
			action(new ProviderOptions(_childProviders));
		}

        protected void Configure(Action<ProviderOptions> action, WebDeployExecuteCondition webDeployExecuteCondition)
        {
            var providerOptions = new ProviderOptions(_childProviders);

            action(providerOptions);
            
            webDeployExecuteCondition.Configure();
            _conditions.Add(webDeployExecuteCondition);
        }

        public WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
			  if (WaitInterval > 0)
			  {
				  webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
			  }

              if (RetryAttempts > 0)
                  webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;

            if(HasConditions())
            {
                if (_conditions.Any(x => x.IsNotExpectedOutcome(webDeployOptions)))
                {
                    deploymentStatus.AddConditionMessage(string.Format("Skipped provider [{0}], because one or more conditions evaluated to false.]", GetType().Name));
                    return deploymentStatus;
                }
            }

            ChildProviders.Reverse();
            ChildProviders.ForEach(provider => provider.Sync(webDeployOptions, deploymentStatus));
            return deploymentStatus;
        }

	    private bool HasConditions()
	    {
	        return _conditions.Count > 0;
	    }
	}
}