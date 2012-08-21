using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.Core.LoadBalancer;

namespace ConDep.Dsl.Core
{
	public class ConDepSetup : ISetupCondep, IValidate
	{
		private readonly List<ConDepOperation> _operations = new List<ConDepOperation>();
	    private ILoadBalance _loadBalancer;
        private readonly ISetupWebDeploy _webDeploySetup;

        public ConDepSetup(ISetupWebDeploy webDeploySetup)
        {
            _webDeploySetup = webDeploySetup;
        }

	    public ISetupWebDeploy WebDeploySetup
	    {
	        get { return _webDeploySetup; }
	    }

	    public void AddOperation(ConDepOperation operation)
	    {
	        CheckLoadBalancerRequirement(operation);
	        _operations.Add(operation);
	    }

	    private void CheckLoadBalancerRequirement(ConDepOperation operation)
	    {
	        if (!(operation is IRequireLoadBalancing)) return;
	        
            if(_loadBalancer == null)
	        {
	            _loadBalancer = GetLoadBalancer();
	        }

	        operation.BeforeExecute = _loadBalancer.BringOffline;
	        operation.AfterExecute = _loadBalancer.BringOnline;
	    }

	    private static ILoadBalance GetLoadBalancer()
	    {
            var loadBalancerLookup = new LoadBalancerLookup(ConDepConfigurator.EnvSettings.LoadBalancer);
            return loadBalancerLookup.GetLoadBalancer();
	    }

	    public bool IsValid(Notification notification)
		{
			return _operations.All(operation => operation.IsValid(notification));
		}

        public WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
            foreach (var operation in _operations)
            {
                operation.Execute(traceLevel, output, outputError, webDeploymentStatus);
            }
			return webDeploymentStatus;
		}
	}
}