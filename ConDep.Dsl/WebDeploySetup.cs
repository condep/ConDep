using System.Collections.Generic;
using StructureMap;

namespace ConDep.Dsl.Core
{
    public class WebDeploySetup : ISetupWebDeploy
    {
        //todo: this prevent uniqe set of providers for context!!!
        private readonly Dictionary<DeploymentServer, WebDeployServerDefinition>  _webDeployDefs = new Dictionary<DeploymentServer, WebDeployServerDefinition>();

        public WebDeployServerDefinition ConfigureServer(DeploymentServer deploymentServer)//, DeploymentUser user)
        {
            //var setup = ObjectFactory.GetInstance<ISetupConDep>() as ConDepSetup;
            if (_webDeployDefs.ContainsKey(deploymentServer))
                return _webDeployDefs[deploymentServer];

            ActiveDeploymentServer = deploymentServer;

            var webDeployServerDefinition = WebDeployServerDefinition.CreateOrGetForServer(deploymentServer);
            _webDeployDefs.Add(deploymentServer, webDeployServerDefinition);
            return webDeployServerDefinition;
        }

        public void ConfigureProvider(IProvide provider)
        {
            //Must be added to the child providers collection of it parent composite provider
            if(provider is WebDeployCompositeProviderBase)
            {
                ((WebDeployCompositeProviderBase)provider).Configure(ActiveDeploymentServer);
            }
            ActiveWebDeployServerDefinition.Providers.Add(provider);


        }

        public void ConfigureProvider(WebDeployCompositeProviderBase provider)
        {
            provider.Configure(ActiveDeploymentServer);
            //provider.ChildProviders.Add();
            //ActiveWebDeployServerDefinition.Providers.Add(provider);
        }

        internal DeploymentServer ActiveDeploymentServer { get; set; }
        
        //public WebDeployServerDefinition ActiveWebDeployServerDefinition
        //{
        //    get
        //    {
        //        return _webDeployDefs[ActiveDeploymentServer];
        //    }
        //}

        public WebDeployServerDefinition ActiveWebDeployServerDefinition {
            get
            {
                return _webDeployDefs[ActiveDeploymentServer];
            }
        }
    }
}