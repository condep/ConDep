using System;
using System.Collections.Generic;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
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

            contextSetup.Execute(options, output, outputError, webDeploymentStatus);
        }
    }
}