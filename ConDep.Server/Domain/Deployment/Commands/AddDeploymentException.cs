using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class  AddDeploymentException : ICommand
    {
        public AddDeploymentException(Guid id, Exception ex)
        {
            Id = id;
            Exception = ex;
        }

        public Guid Id { get; private set; }
        public Exception Exception { get; private set; }
    }
}