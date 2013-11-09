using System;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.Remote
{
    public class PowerShellErrors : Exception
    {
        private readonly List<Exception> _exceptions = new List<Exception>();

        public void Add(Exception exception)
        {
            _exceptions.Add(exception);
        }

        public override string Message
        {
            get
            {
                if (_exceptions.Count > 1)
                {
                    int counter = 1;
                    return _exceptions.Aggregate("",
                                                 (current, exception) =>
                                                 current +
                                                 string.Format("Exception #{0}: " + exception + "\n", counter++));
                }
                else
                {
                    return _exceptions.Aggregate("", (current, exception) => current + exception);
                }
            }
        }

        public override string ToString()
        {
            return Message;
        }
    }
}