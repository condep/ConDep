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

        public void Define(Action<IProvideForCustomIisDefinition> iisDefinition)
        {
            throw new NotImplementedException();
        }

        public void SyncFromExistingServer(string iisServer, Action<IProvideForExistingIisServer> sync)
        {
            throw new NotImplementedException();
        }
    }
}