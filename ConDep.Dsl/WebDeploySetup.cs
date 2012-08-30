using System.Collections.Generic;
using StructureMap;

namespace ConDep.Dsl
{
    public class WebDeploySetup : ISetupWebDeploy
    {
        //todo: this prevent uniqe set of providers for context!!!
        //private readonly Dictionary<string, WebDeployServerDefinition>  _webDeployDefs = new Dictionary<string, WebDeployServerDefinition>();

        public WebDeployServerDefinition ConfigureServer(DeploymentServer deploymentServer)
        {
            //if (_webDeployDefs.ContainsKey(deploymentServer.ServerName + "-" + context))
            //{
            //    ActiveServerContextKey = deploymentServer.ServerName + "-" + context;
            //    ActiveDeploymentServer = deploymentServer;
            //    return _webDeployDefs[deploymentServer.ServerName + "-" + context];
            //}

            //ActiveServerContextKey = deploymentServer.ServerName + "-" + context;
            ActiveDeploymentServer = deploymentServer;

            ActiveWebDeployServerDefinition = WebDeployServerDefinition.CreateOrGetForServer(deploymentServer);
            //_webDeployDefs.Add(deploymentServer.ServerName + "-" + context, webDeployServerDefinition);
            return ActiveWebDeployServerDefinition;
        }

        internal DeploymentServer ActiveDeploymentServer { get; set; }

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

        internal string ActiveServerContextKey { get; set; }
        
        //public WebDeployServerDefinition ActiveWebDeployServerDefinition
        //{
        //    get
        //    {
        //        return _webDeployDefs[ActiveDeploymentServer];
        //    }
        //}

        public WebDeployServerDefinition ActiveWebDeployServerDefinition { get; private set; }
    }
}