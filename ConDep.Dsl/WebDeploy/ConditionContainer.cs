using System.Collections.Generic;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.WebDeploy
{
    internal class ConditionContainer<T> where T : IProvideOptions, new()
    {
        private readonly List<IProvide> _childProviders;
        private readonly ServerConfig _server;
        private WebDeployExecuteCondition<T> _condition;

        public ConditionContainer(WebDeployExecuteCondition<T> condition, List<IProvide> childProviders, ServerConfig server)
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