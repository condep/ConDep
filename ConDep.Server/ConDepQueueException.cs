using System;

namespace ConDep.Server
{
    public class ConDepQueueException : Exception
    {
        public ConDepQueueException(string message)
            : base(message) {}

        public ConDepQueueException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}