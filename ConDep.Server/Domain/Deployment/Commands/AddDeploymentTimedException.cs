using System;
using ConDep.Server.Domain.Deployment.Model;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class AddDeploymentTimedException : ICommand
    {
        public AddDeploymentTimedException(Guid id, TimedException ex)
        {
            Id = id;
            TimedException = ex;
        }

        public Guid Id { get; private set; }
        public TimedException TimedException { get; private set; }
    }
}