using System;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Builders
{
    public class IisOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public IisOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void FromExistingServer(string sourceServer, Action<IProvideForExistingIisServer> action)
        {
            throw new NotImplementedException();
        }

        public void FromCustomDefinition(Action<IProvideForCustomIisDefinition> action)
        {
            throw new NotImplementedException();
        }
    }
}