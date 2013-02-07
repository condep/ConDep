using System;

namespace ConDep.WebQ.Client
{
    public class WebQItemNoLongerInQueueException : Exception
    {
        public WebQItemNoLongerInQueueException() : base("Item no longer present in queue.")
        {
        }
    }
}