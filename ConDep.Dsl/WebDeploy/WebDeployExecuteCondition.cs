using System;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.WebDeploy
{
    public class WebDeployExecuteCondition<T> : IProvideConditions, IValidate where T : IProvideOptions, new()
    {
        private enum ExpectedOutcome
        {
            Success,
            Failure
        }

        private readonly Action<T> _action;
        private readonly ExpectedOutcome _expectedOutcome;
        private readonly List<IProvide> _providers = new List<IProvide>();
        private DeploymentServer _server;

        private WebDeployExecuteCondition(Action<T> action, ExpectedOutcome expectedOutcome)
        {
            _action = action;
            _expectedOutcome = expectedOutcome;
        }

        //public void Configure()
        //{
        //    throw new NotImplementedException();
        //    //var providerOptions = new ProviderOptions(_providers);
        //    //_action(providerOptions);
        //}

        public static WebDeployExecuteCondition<T> IsSuccess(Action<T> action)
        {
            return new WebDeployExecuteCondition<T>(action, ExpectedOutcome.Success);
        }

        public static WebDeployExecuteCondition<T> IsFailure(Action<T> action)
        {
            return new WebDeployExecuteCondition<T>(action, ExpectedOutcome.Failure);
        }

        public bool HasExpectedOutcome(WebDeployOptions webDeployOptions)
        {
            var deploymentStatus = new WebDeploymentStatus();
            bool exception = false;

            try
            {
                _providers.ForEach(provider => provider.Sync(webDeployOptions, deploymentStatus));
            }
            catch
            {
                exception = true;    
            }

            switch(_expectedOutcome)
            {
                case ExpectedOutcome.Success:
                    return !deploymentStatus.HasErrors || !exception;
                case ExpectedOutcome.Failure:
                    return deploymentStatus.HasErrors || exception;
                default:
                    throw new UnsupportedOutcomeException();
            }
        }

        public bool IsValid(Notification notification)
        {
            return _providers.All(provider => provider.IsValid(notification));
        }

        public void Configure(DeploymentServer arrServer)
        {
            _server = arrServer;
            var options = new T();
            options.AddProviderAction = AddChildProvider;
            _action(options);
        }

        public void AddChildProvider(IProvide provider)
        {
            if (provider is WebDeployCompositeProviderBase)
            {
                ((WebDeployCompositeProviderBase)provider).Configure(_server);
            }
            _providers.Add(provider);
        }
    }
}