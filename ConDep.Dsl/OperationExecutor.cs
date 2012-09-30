using System;
using System.Collections.Generic;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public class OperationExecutor
    {
        private readonly List<ConDepOperationBase> _operations;
        private readonly ConDepOptions _options;
        private readonly WebDeploymentStatus _webDeploymentStatus;
        private readonly ConDepContext _context;

        public OperationExecutor(List<ConDepOperationBase> operations, ConDepOptions options, WebDeploymentStatus webDeploymentStatus, ConDepContext context)
        {
            _operations = operations;
            _options = options;
            _webDeploymentStatus = webDeploymentStatus;
            _context = context;
        }

        public WebDeploymentStatus Execute()
        {
            foreach (var operation in _operations)
            {
                ExecuteOperation(operation);
                if (_webDeploymentStatus.HasErrors) return _webDeploymentStatus;
            }
            return _webDeploymentStatus;
        }

        private void ExecuteOperation(ConDepOperationBase operation)
        {
            if (operation is ConDepContextOperationPlaceHolder)
            {
                ExecuteContextPlaceholderOperation(_options, _webDeploymentStatus, operation);
            }
            else
            {
                Logger.TeamCityBlockStart("Executing ConDep operation " + operation.GetType().Name);
                operation.Execute(_webDeploymentStatus);
                Logger.TeamCityBlockEnd("Executing ConDep operation " + operation.GetType().Name);
            }
        }

        private void ExecuteContextPlaceholderOperation(ConDepOptions options, WebDeploymentStatus webDeploymentStatus, ConDepOperationBase operation)
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

            Logger.TeamCityBlockStart("Executing ConDep context " + ((ConDepContextOperationPlaceHolder)operation).ContextName);
            contextSetup.Execute(options, webDeploymentStatus);
            Logger.TeamCityBlockEnd("Executing ConDep context " + ((ConDepContextOperationPlaceHolder)operation).ContextName);
        }
    }
}