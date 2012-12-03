using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.Experimental.Core.Impl
{
    public class LocalSequenceManager : IManageLocalSequence
    {
        private readonly List<LocalOperation> _sequence = new List<LocalOperation>();

        public void Add(LocalOperation localOperation)
        {
            _sequence.Add(localOperation);
        }

        public IReportStatus Execute(IReportStatus status)
        {
            foreach(var element in _sequence)
            {
                try
                {
                    Logger.LogSectionStart(element.GetType().Name);
                    element.Execute(status);
                    if (status.HasErrors)
                        return status;
                }
                finally
                {
                    Logger.LogSectionEnd(element.GetType().Name);
                }
            }
            return status;
        }

        public bool IsValid(Notification notification)
        {
            return !_sequence.Any(x => x.IsValid(notification) == false);
        }
    }
}