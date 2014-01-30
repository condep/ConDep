using System;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Domain.Queue.Model;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class UpdateQueueStatus : ICommand
    {
        public UpdateQueueStatus(Guid id, QueueStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; private set; }
        public QueueStatus Status { get; private set; }
    }
}