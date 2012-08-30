using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.LoadBalancer;

namespace ConDep.Dsl
{
    public class ConDepSetup : ISetupConDep, IValidate, IProvideForSetup
	{
		private readonly List<ConDepOperationBase> _operations = new List<ConDepOperationBase>();
	    private ILoadBalance _loadBalancer;
        private readonly ISetupWebDeploy _webDeploySetup;
        private readonly ConDepEnvironmentSettings _envSettings;
        private readonly LoadBalancerLookup _loadBalancerLookup;
        private readonly ConDepContext _context;

        public ConDepSetup(ISetupWebDeploy webDeploySetup, ConDepEnvironmentSettings envSettings, LoadBalancerLookup loadBalancerLookup, ConDepContext context)
        {
            _webDeploySetup = webDeploySetup;
            _envSettings = envSettings;
            _loadBalancerLookup = loadBalancerLookup;
            _context = context;
        }

        public ConDepContext Context { get { return _context; } }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }

        public ConDepEnvironmentSettings EnvSettings { get { return _envSettings; } }

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
            if (options.PrintSequence)
            {
                PrintExecutionSequence(options, 0);
                return webDeploymentStatus;
            }

            var operationExecutor = new OperationExecutor(_operations, options, output, outputError, webDeploymentStatus, _context);
            return operationExecutor.Execute();
            //return ExecuteAllOperations(options, output, outputError, webDeploymentStatus);
        }

        public WebDeploymentStatus ExecuteAllContextOperations(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            foreach (var operation in _operations)
            {
                operation.Execute(traceLevel, output, outputError, webDeploymentStatus);
            }
			return webDeploymentStatus;
        }

        public void PrintExecutionSequence(ConDepOptions options, int level)
        {
            if(options.HasContext())
            {
                foreach (var operation in _operations)
                {
                    if (operation is ConDepContextOperationPlaceHolder)
                    {
                        if(options.Context == ((ConDepContextOperationPlaceHolder)operation).ContextName)
                        {
                            var contextSetup = _context[options.Context];
                            contextSetup.PrintExecutionSequence(options, level);
                        }
                    }
                    else
                    {
                        operation.PrintExecutionSequence(Console.Out, level);
                    }
                }
            }
            else
            {
                foreach (var operation in _operations)
                {
                    if (operation is ConDepContextOperationPlaceHolder)
                    {
                        var contextSetup = _context[((ConDepContextOperationPlaceHolder)operation).ContextName];
                        contextSetup.PrintExecutionSequence(options, level);
                    } 
                    else
                    {
                        operation.PrintExecutionSequence(Console.Out, level);
                    }
                }
            }
        }
	}

    public class OperationExecutor
    {
        private readonly List<ConDepOperationBase> _operations;
        private readonly ConDepOptions _options;
        private readonly EventHandler<WebDeployMessageEventArgs> _output;
        private readonly EventHandler<WebDeployMessageEventArgs> _outputError;
        private readonly WebDeploymentStatus _webDeploymentStatus;
        private readonly ConDepContext _context;

        public OperationExecutor(List<ConDepOperationBase> operations, ConDepOptions options, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus, ConDepContext context)
        {
            _operations = operations;
            _options = options;
            _output = output;
            _outputError = outputError;
            _webDeploymentStatus = webDeploymentStatus;
            _context = context;
        }

        public WebDeploymentStatus Execute()
        {
            foreach (var operation in _operations)
            {
                ExecuteOperation(operation);
            }
            return _webDeploymentStatus;
        }

        private void ExecuteOperation(ConDepOperationBase operation)
        {
            if (operation is ConDepContextOperationPlaceHolder)
            {
                ExecuteContextPlaceholderOperation(_options, _output, _outputError, _webDeploymentStatus, operation);
            }
            else
            {
                operation.Execute(_options.TraceLevel, _output, _outputError, _webDeploymentStatus);
            }
        }

        private void ExecuteContextPlaceholderOperation(ConDepOptions options, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError,
                                                        WebDeploymentStatus webDeploymentStatus, ConDepOperationBase operation)
        {
            ISetupConDep contextSetup;

            if (options.HasContext())
            {
                if (((ConDepContextOperationPlaceHolder)operation).ContextName == options.Context)
                {
                    contextSetup = _context[options.Context];
                }
                else
                {
                    return;
                }
            }
            else
            {
                contextSetup = _context[((ConDepContextOperationPlaceHolder)operation).ContextName];
            }

            contextSetup.ExecuteAllContextOperations(options.TraceLevel, output, outputError, webDeploymentStatus);
        }
    }
}