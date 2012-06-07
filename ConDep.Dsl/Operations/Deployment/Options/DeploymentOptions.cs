using System;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Builders
{
	public class DeploymentOptions
	{
		private readonly SetupOperation _setupOperation;
        private LoadBalancerOptions _loadBalancer;
	    private WebDeploymentStatus _webDeploymentStatus;

	    public DeploymentOptions(SetupOperation setupOperation)
		{
			_setupOperation = setupOperation;
		}

	    public LoadBalancerOptions LoadBalancer
	    {
	        get { return _loadBalancer ?? (_loadBalancer = new LoadBalancerOptions(new LoadBalancerOperation())); }
	    }

        public IProvideForDeployment Deployment
        {
            get
            {
                var webDeployDefinition = new WebDeployDefinition();
                var webDeployOperation = new DeploymentOperation(webDeployDefinition);
                AddOperation(webDeployOperation);

                return new DeploymentProviderOptions(webDeployDefinition);
            }
        }

        public InfrastructureOptions Infrastructure { get; set; }

	    public PreProcessingOptions PreProcessing { get; set; }

	    public void AddOperation(IOperateConDep operation)
		{
			_setupOperation.AddOperation(operation);
		}
	}

    public class PreProcessingOptions
    {
    }

    public class InfrastructureOptions
    {
        public InfrastructureIisOptions IIS { get; set; }
        public InfrastructureWindowsOptions Windows { get; set; }
    }

    public class InfrastructureWindowsOptions
    {
    }

    public class InfrastructureIisOptions
    {
        private WebDeployDefinition _webDeployDefinition;

        public InfrastructureIisOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void Define(Action<IProvideForCustomIisDefinition> iisDefinition)
        {
            iisDefinition(new ProviderOptions(_webDeployDefinition.Providers));
        }
    }
}