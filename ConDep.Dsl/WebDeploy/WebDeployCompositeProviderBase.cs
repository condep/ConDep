using System;
using System.Linq;
using System.Collections.Generic;

namespace ConDep.Dsl.WebDeploy
{
	public abstract class WebDeployCompositeProviderBase : IProvide
	{
	    private readonly List<IProvide> _childProviders = new List<IProvide>();
	    private DeploymentServer _server;
	    //private readonly List<WebDeployExecuteCondition> _conditions = new List<WebDeployExecuteCondition>();

	    public IEnumerable<IProvide> ChildProviders { get { return _childProviders; } }
        //public IEnumerable<WebDeployExecuteCondition> ExecuteConditions { get { return _conditions; } }
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }

		public abstract bool IsValid(Notification notification);

		public int WaitInterval { get; set; }
        public int RetryAttempts { get; set; }

        public abstract void Configure(DeploymentServer server);

        public virtual void BeforeExecute(EventHandler<WebDeployMessageEventArgs> output)
        {
            output(this, new WebDeployMessageEventArgs { Message = string.Format("Executing {0}", GetType().Name), Level = System.Diagnostics.TraceLevel.Info });
        }
        public virtual void AfterExecute(EventHandler<WebDeployMessageEventArgs> output)
        {
            output(this, new WebDeployMessageEventArgs { Message = string.Format("{0} : Execution finished for provider [{1}]", DateTime.Now.ToLongTimeString(), this.GetType().Name), Level = System.Diagnostics.TraceLevel.Info });
        }

	    public void AddChildProvider(IProvide provider)
        {
            if (provider is WebDeployCompositeProviderBase)
            {
                ((WebDeployCompositeProviderBase)provider).Configure(_server);
            }
            _childProviders.Add(provider);
        }

        protected void Configure<T>(DeploymentServer server, Action<IProvide> addProviderAction, Action<T> action) where T : IProvideOptions, new()
        {
            _server = server;
            //var options = ObjectFactory.GetInstance<T>();
            var options = new T { AddProviderAction = addProviderAction };
            //options.WebDeploySetup.ConfigureServer(server);


            //options.Provider = provider; //Can possible send in function in provider to add subProviders?
            //var compositeOptions = new CompositeProviderOptions(_childProviders, server);
            //action(options, compositeOptions);
            action(options);
        }

        //protected void Configure(Action<ProviderOptions> action, WebDeployExecuteCondition webDeployExecuteCondition)
        //{
        //    var providerOptions = new ProviderOptions(_childProviders);

        //    action(providerOptions);
            
        //    webDeployExecuteCondition.Configure();
        //    _conditions.Add(webDeployExecuteCondition);
        //}

        public WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
			  if (WaitInterval > 0)
			  {
				  webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
			  }

              if (RetryAttempts > 0)
                  webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;

            //if(HasConditions())
            //{
            //    if (_conditions.Any(x => x.IsNotExpectedOutcome(webDeployOptions)))
            //    {
            //        deploymentStatus.AddConditionMessage(string.Format("Skipped provider [{0}], because one or more conditions evaluated to false.]", GetType().Name));
            //        return deploymentStatus;
            //    }
            //}

            ChildProviders.Reverse();

            ChildProviders.ToList().ForEach(provider => provider.Sync(webDeployOptions, deploymentStatus));
            return deploymentStatus;
        }

        //private bool HasConditions()
        //{
        //    return _conditions.Count > 0;
        //}
	}
}