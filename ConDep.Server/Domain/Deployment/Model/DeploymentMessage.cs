using System;

namespace ConDep.Server.Model.DeploymentAggregate
{
    public class DeploymentMessage
    {
        private DeploymentMessage()
        {
            
        }

        public DeploymentMessage(string message)
        {
            Message = message;
            DateUtc = DateTime.UtcNow;
        }

        public DeploymentMessage(DateTime utcDateTime, string message)
        {
            Message = message;
            DateUtc = utcDateTime;
        }

        public DateTime DateUtc { get; private set; }
        public string Message { get; private set; }
    }
}