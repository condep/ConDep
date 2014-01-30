using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class ProcessEnvironmentQueue : ICommand
    {
        public ProcessEnvironmentQueue(string environment)
        {
            Id = Guid.NewGuid();
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; private set; }
    }
}