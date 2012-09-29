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
            Logger.TeamCityBlockStart("ConDep");
            Logger.TeamCityProgressMessage("Executing ConDep");
            Logger.Info("Starting execution of ConDep");

            foreach (var operation in _operations)
            {
                ExecuteOperation(operation);
                if (_webDeploymentStatus.HasErrors) return _webDeploymentStatus;
            }
            Logger.Info("Finished executing ConDep");
            Logger.TeamCityBlockEnd("ConDep");
            return _webDeploymentStatus;
        }

        private void ExecuteOperation(ConDepOperationBase operation)
        {
            if (operation is ConDepContextOperationPlaceHolder)
            {
                Logger.TeamCityBlockStart(((ConDepContextOperationPlaceHolder)operation).ContextName);
                ExecuteContextPlaceholderOperation(_options, _webDeploymentStatus, operation);
                Logger.TeamCityBlockEnd(((ConDepContextOperationPlaceHolder)operation).ContextName);
            }
            else
            {
                Logger.TeamCityBlockStart(operation.GetType().Name);
                operation.Execute(_webDeploymentStatus);
                Logger.TeamCityBlockEnd(operation.GetType().Name);
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

            contextSetup.Execute(options, webDeploymentStatus);
        }
    }
}