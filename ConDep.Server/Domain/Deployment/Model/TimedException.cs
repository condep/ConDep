using System;

namespace ConDep.Server.Domain.Deployment.Model
{
    public class TimedException
    {
        public DateTime DateTime { get; set; }
        public Exception Exception { get; set; }
    }
}