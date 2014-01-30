using System;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment
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