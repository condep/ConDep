using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core.Impl
{
    public class LocalSequenceManager : IManageLocalSequence
    {
        private readonly List<ConDepOperationBase> _sequence = new List<ConDepOperationBase>();
 
        public void Add(ConDepOperationBase localOperation)
        {
            _sequence.Add(localOperation);
        }

        public IReportStatus Execute(IReportStatus status)
        {
            foreach(var element in _sequence)
            {
                element.Execute(status);
                if (status.HasErrors)
                    return status;
            }
            return status;
        }

        public bool IsValid(Notification notification)
        {
            return !_sequence.Any(x => x.IsValid(notification) == false);
        }
    }
}