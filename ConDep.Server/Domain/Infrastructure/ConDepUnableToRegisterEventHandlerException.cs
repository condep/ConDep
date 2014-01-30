using System;

namespace ConDep.Server.Domain.Infrastructure
{
    public class ConDepUnableToRegisterEventHandlerException : Exception
    {
        public ConDepUnableToRegisterEventHandlerException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}