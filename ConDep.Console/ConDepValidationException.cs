using System;

namespace ConDep.Console
{
    public class ConDepValidationException : Exception
    {
        public ConDepValidationException(string message) : base(message)
        {
        }
    }
}