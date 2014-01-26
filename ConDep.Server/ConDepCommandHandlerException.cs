using System;

namespace ConDep.Server
{
    public class ConDepCommandHandlerException : Exception
    {
        public ConDepCommandHandlerException(string message) : base(message)
        {
            
        }
    }
}