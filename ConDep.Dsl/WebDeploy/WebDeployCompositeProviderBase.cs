using System;
using System.Linq;
using System.Collections.Generic;

namespace ConDep.Dsl.WebDeploy
{
    public abstract class WebDeployCompositeProviderBase : IProvide
    {
        private readonly List<IProvide> _childProviders = new List<IProvide>();
        private DeploymentServer _server;
        private List<IProvideConditions> _conditions = new List<IProvideConditions>();

        public IEnumerable<IProvide> ChildProviders { get { return _childProviders; } }
        public string SourcePath { get; set; }
        public virtual string DestinationPath { get; set; }
        public abstract bool IsValid(Notification notification);
        public int WaitInterval { get; set; }
        public int RetryAttempts { get; set; }

        public abstract void Configure(DeploymentServer arrServer);

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

        protected void Configure<T>(DeploymentServer arrServer, Action<T> action) where T : IProvideOptions, new()
        {
            _server = arrServer;
            var options = new T { AddProviderAction = AddChildProvider };
            action(options);
        }

        protected void Configure<T1, T2>(DeploymentServer server, Action<T1> action, WebDeployExecuteCondition<T2> condition)
            where T1 : IProvideOptions, new()
            where T2 : IProvideOptions, new()
        {
            _server = server;

            condition.Configure(server);
            var conditionContainer = new ConditionContainer<T2>(condition, _childProviders, _server);

            var options = new T1 { AddProviderAction = conditionContainer.AddConditionProvider };
            action(options);
        }

        public void AddCondition(IProvideConditions condition)
        {
            _conditions.Add(condition);
        }

        public WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
            if (WaitInterval > 0)
            {
                webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
            }

            if (RetryAttempts > 0)
                webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;

            if (HasConditions())
            {
                if (_conditions.Any(condition => !condition.HasExpectedOutcome(webDeployOptions))) return deploymentStatus;
            }

            ChildProviders.Reverse();

            ChildProviders.ToList().ForEach(provider => provider.Sync(webDeployOptions, deploymentStatus));
            return deploymentStatus;
        }

        private bool HasConditions()
        {
            return _conditions.Count > 0;
        }
    }
}