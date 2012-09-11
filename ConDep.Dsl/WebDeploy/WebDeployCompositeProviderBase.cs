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

        //private readonly List<IProvide> _conditions = new List<IProvide>();

        public IEnumerable<IProvide> ChildProviders { get { return _childProviders; } }
        //public IEnumerable<WebDeployExecuteCondition> ExecuteConditions { get { return _conditions; } }
        public string SourcePath { get; set; }
        public virtual string DestinationPath { get; set; }

        public abstract bool IsValid(Notification notification);

        public int WaitInterval { get; set; }

        //public IProvide Condition { get; set; }

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

        //public void AddConditionProvider(IProvide provider)
        //{
        //    if (provider is WebDeployCompositeProviderBase)
        //    {
        //        ((WebDeployCompositeProviderBase)provider).Configure(_server);
        //    }
        //    _conditions.Add(provider);
        //}

        protected void Configure<T>(DeploymentServer arrServer, Action<T> action) where T : IProvideOptions, new()
        {
            _server = arrServer;
            var options = new T { AddProviderAction = AddChildProvider };
            action(options);
        }

        protected void Configure<T1, T2>(DeploymentServer arrServer, Action<T1> action, WebDeployExecuteCondition<T2> condition)
            where T1 : IProvideOptions, new()
            where T2 : IProvideOptions, new()
        {
            _server = arrServer;

            condition.Configure(arrServer);
            var conditionContainer = new ConditionContainer<T2>(condition, _childProviders, _server);

            //var conditionOptions = new T2 { AddProviderAction = conditionContainer.AddCondition };
            //condition(conditionOptions);

            var options = new T1 { AddProviderAction = conditionContainer.AddConditionProvider };
            action(options);

            //condition.Configure(arrServer);
            //_conditions.Add(condition);
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

            //if (HasConditions())
            //{
            try
            {
                if (_conditions.Count > 0)
                {
                    foreach (var condition in _conditions)
                    {
                        if (!condition.HasExpectedOutcome(webDeployOptions))
                        {
                            return deploymentStatus;
                        }
                    }
                }
            }
            catch
            {
                return deploymentStatus;
            }

                //if (_conditions.Any(x => x.IsNotExpectedOutcome(webDeployOptions)))
                //{
                //    deploymentStatus.AddConditionMessage(string.Format("Skipped provider [{0}], because one or more conditions evaluated to false.]", GetType().Name));
                //    return deploymentStatus;
                //}
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

    public class ConditionContainer<T> where T : IProvideOptions, new()
    {
        private readonly List<IProvide> _childProviders;
        private readonly DeploymentServer _server;
        private WebDeployExecuteCondition<T> _condition;

        public ConditionContainer(WebDeployExecuteCondition<T> condition, List<IProvide> childProviders, DeploymentServer server)
        {
            _condition = condition;
            _childProviders = childProviders;
            _server = server;
        }

        public void AddConditionProvider(IProvide provider)
        {
            provider.AddCondition(_condition);

            if (provider is WebDeployCompositeProviderBase)
            {
                ((WebDeployCompositeProviderBase)provider).Configure(_server);
            }
            _childProviders.Add(provider);
        }

        //public void AddCondition(IProvide conditionProvider)
        //{
        //    _condition = conditionProvider;

        //    if (conditionProvider is WebDeployCompositeProviderBase)
        //    {
        //        ((WebDeployCompositeProviderBase)conditionProvider).Configure(_server);
        //    }
        //}
    }
}