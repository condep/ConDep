using System.Collections.Generic;

namespace ConDep.Dsl.WebDeploy
{
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
    }
}