using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.LoadBalancer;
using StructureMap;

namespace ConDep.Dsl.Core
{
    public class ConDepSetup : ISetupConDep, IValidate, IProvideForSetup
	{
		private readonly List<ConDepOperationBase> _operations = new List<ConDepOperationBase>();
	    private ILoadBalance _loadBalancer;
        private readonly ISetupWebDeploy _webDeploySetup;
        private readonly ConDepEnvironmentSettings _envSettings;
        private readonly LoadBalancerLookup _loadBalancerLookup;
        private readonly ConDepAppContext _appContext;

        public ConDepSetup(ISetupWebDeploy webDeploySetup, ConDepEnvironmentSettings envSettings, LoadBalancerLookup loadBalancerLookup, ConDepAppContext appContext)
        {
            _webDeploySetup = webDeploySetup;
            _envSettings = envSettings;
            _loadBalancerLookup = loadBalancerLookup;
            _appContext = appContext;
        }

        public ConDepAppContext Context { get { return _appContext; } }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }

        public ConDepEnvironmentSettings EnvSettings { get { return _envSettings; } }

        //public ConDepAppContext AppContext
        //{
        //    get { return _appContext; }
        //}

        public void AddOperation(ConDepOperationBase operation)
	    {
	        CheckLoadBalancerRequirement(operation);
	        _operations.Add(operation);
	    }

	    private void CheckLoadBalancerRequirement(ConDepOperationBase operation)
	    {
	        if (!(operation is IRequireLoadBalancing)) return;
	        
            if(_loadBalancer == null)
	        {
	            _loadBalancer = GetLoadBalancer();
	        }

	        operation.BeforeExecute = _loadBalancer.BringOffline;
	        operation.AfterExecute = _loadBalancer.BringOnline;
	    }

	    private ILoadBalance GetLoadBalancer()
	    {
            return _loadBalancerLookup.GetLoadBalancer();
	    }

	    public bool IsValid(Notification notification)
		{
			return _operations.All(operation => operation.IsValid(notification));
		}

        public WebDeploymentStatus Execute(ConDepOptions options, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
            if (options.HasContext())
            {
                var contextSetup = _appContext[options.Context];
                return contextSetup.ExecuteContext(options.TraceLevel, output, outputError, webDeploymentStatus);
            }

            foreach (var operation in _operations)
            {
                operation.Execute(options.TraceLevel, output, outputError, webDeploymentStatus);
            }
			return webDeploymentStatus;
		}

        public WebDeploymentStatus ExecuteContext(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            foreach (var operation in _operations)
            {
                operation.Execute(traceLevel, output, outputError, webDeploymentStatus);
            }
			return webDeploymentStatus;
        }

        //public void ConfigureProvider<T>(Action<ProviderOptions<T>> options) where T: new()
        //{
        //    foreach(var deploymentServer in EnvSettings.Servers)
        //    {
        //        var serverDefinition = _webDeploySetup.ConfigureServer(deploymentServer, EnvSettings.DeploymentUser);
        //        var webDeployOperation = new WebDeployOperation(serverDefinition);
        //        AddOperation(webDeployOperation);
        //        options(new T());
        //    }
        //}
	}

    public interface IProvideForSetup
    {
    }
}