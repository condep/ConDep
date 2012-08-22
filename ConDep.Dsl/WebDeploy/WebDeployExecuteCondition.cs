using System;
using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    public class WebDeployExecuteCondition<T>
    {
        private enum ExpectedOutcome
        {
            Success,
            Failure
        }

        private readonly Action<T> _action;
        private readonly ExpectedOutcome _expectedOutcome;
        private readonly List<IProvide> _providers = new List<IProvide>();

        private WebDeployExecuteCondition(Action<T> action, ExpectedOutcome expectedOutcome)
        {
            _action = action;
            _expectedOutcome = expectedOutcome;
        }

        public void Configure()
        {
            throw new NotImplementedException();
            //var providerOptions = new ProviderOptions(_providers);
            //_action(providerOptions);
        }

        public static WebDeployExecuteCondition<T> IsSuccess(Action<T> action)
        {
            throw new NotImplementedException();
            //return new WebDeployExecuteCondition(action, ExpectedOutcome.Success);
        }

        public static WebDeployExecuteCondition<T> IsFailure(Action<T> action)
        {
            throw new NotImplementedException();
            //return new WebDeployExecuteCondition(action, ExpectedOutcome.Failure);
        }

        public bool IsNotExpectedOutcome(WebDeployOptions webDeployOptions)
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
    }
}