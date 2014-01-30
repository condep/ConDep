using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class AddDeploymentExecutionEvent : ICommand
    {
        public AddDeploymentExecutionEvent(Guid id, string message)
        {
            Id = id;
            Message = message;
        }

        public Guid Id { get; private set; }
        public string Message { get; private set; }
    }
}