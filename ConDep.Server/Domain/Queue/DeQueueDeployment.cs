using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue
{
    public class DeQueueDeployment : ICommand
    {
        public DeQueueDeployment(Guid id, string environment)
        {
            Id = id;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; set; }
    }
}