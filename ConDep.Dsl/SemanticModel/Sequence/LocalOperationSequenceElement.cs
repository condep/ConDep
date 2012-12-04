using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class LocalOperationSequenceElement : ISequenceElement
    {
        private readonly LocalOperation _operation;

        public LocalOperationSequenceElement(LocalOperation operation)
        {
            _operation = operation;
        }

        public IReportStatus Execute(IReportStatus status)
        {
            try
            {
                Logger.LogSectionStart(_operation.GetType().Name);
                return _operation.Execute(status);
            }
            finally
            {
                Logger.LogSectionEnd(_operation.GetType().Name);
            }
        }

        public bool IsValid(Notification notification)
        {
            return _operation.IsValid(notification);
        }
    }
}