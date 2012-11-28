using System;
using System.Collections.Generic;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    internal class OperationExecutor
    {
        private readonly List<ConDepOperationBase> _operations;
        private readonly ConDepOptions _options;
        private readonly IReportStatus _webDeploymentStatus;
        private readonly ConDepContext _context;

        public OperationExecutor(List<ConDepOperationBase> operations, ConDepOptions options, IReportStatus webDeploymentStatus, ConDepContext context)
        {
            _operations = operations;
            _options = options;
            _webDeploymentStatus = webDeploymentStatus;
            _context = context;
        }

        public IReportStatus Execute()
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
                Logger.LogSectionStart(operation.GetType().Name);
                operation.Execute(_webDeploymentStatus);
                Logger.LogSectionEnd(operation.GetType().Name);
            }
        }

        private void ExecuteContextPlaceholderOperation(ConDepOptions options, IReportStatus status, ConDepOperationBase operation)
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

            Logger.LogSectionStart(((ConDepContextOperationPlaceHolder)operation).ContextName + " context");
            contextSetup.Execute(options, status);
            Logger.LogSectionEnd(((ConDepContextOperationPlaceHolder)operation).ContextName + " context");
        }
    }
}