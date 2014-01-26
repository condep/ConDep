using System;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.QueueAggregate;

namespace ConDep.Server.Commands
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