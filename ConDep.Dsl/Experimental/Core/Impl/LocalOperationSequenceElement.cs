using System;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core.Impl
{
    public class LocalOperationSequenceElement : ISequenceElement
    {
        private readonly ConDepOperationBase _operation;

        public LocalOperationSequenceElement(ConDepOperationBase operation)
        {
            _operation = operation;
        }

        public IReportStatus Execute(IReportStatus status)
        {
            return _operation.Execute(status);
        }

        public bool IsValid(Notification notification)
        {
            return _operation.IsValid(notification);
        }
    }
}