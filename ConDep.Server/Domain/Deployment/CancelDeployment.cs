using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment
{
    public class CancelDeployment : ICommand
    {
        public CancelDeployment(Guid id)
        {
            Id = id;        
        }

        public Guid Id { get; private set; }
    }
}