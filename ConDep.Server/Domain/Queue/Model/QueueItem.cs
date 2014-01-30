using System;
using System.Collections.Generic;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Model
{
    public class QueueItem : IAggregateRoot, IPublishEvents
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public QueueItem()
        {
            
        }

        public QueueItem(string environment, string module, string artifact)
        {
            DeploymentId = Guid.NewGuid();
            CreatedUtc = DateTime.UtcNow;
            QueueStatus = QueueStatus.Waiting;
            Environment = environment;
            Module = module;
            Artifact = artifact;

            AddEvent(new DeploymentQueued(DeploymentId, environment));
        }

        public string Environment { get; private set; }
        public string Module { get; private set; }
        public string Artifact { get; private set; }

        public Guid DeploymentId { get; private set; }
        public DateTime CreatedUtc { get; private set; }
        public DateTime FinishedUtc { get; private set; }
        public QueueStatus QueueStatus { get; set; }

        public IEnumerable<IEvent> Events { get { return _events; } }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void AddEvent(IEvent @event)
        {
            _events.Add(@event);
        }

        public void SetInProgress()
        {
            QueueStatus = QueueStatus.DeploymentInProgress;
            AddEvent(new DeploymentQueueItemInProgress(DeploymentId, Environment));
        }

        public void SetReadyForDeployment()
        {
            QueueStatus = QueueStatus.ReadyForDeployment;
            AddEvent(new EnvironmentQueueReadyForDeployment(DeploymentId, Environment, Module, Artifact));
        }
    }
}