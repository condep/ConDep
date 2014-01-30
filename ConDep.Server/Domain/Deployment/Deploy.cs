using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment
{
    public class Deploy : ICommand
    {
        public Deploy(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}