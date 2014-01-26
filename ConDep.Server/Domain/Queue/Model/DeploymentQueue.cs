using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.DomainEvents;
using ConDep.Server.Infrastructure;
using Raven.Imports.Newtonsoft.Json;

namespace ConDep.Server.Domain.Queue.Model
{
    public class DeploymentQueue : IAggregateRoot, IPublishEvents
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public DeploymentQueue()
        {
            EnvironmentQueues = new List<EnvironmentQueue>();
        }

        [JsonIgnore]
        public Guid? Etag { get; set; }
        
        public List<EnvironmentQueue> EnvironmentQueues { get; set; }

        public void Process()
        {
            //var queueEnvironments = Session
            //    .Query<EnvironmentQueue_Environments.Result, EnvironmentQueue_Environments>()
            //    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
            //    .AsProjection<EnvironmentQueue_Environments.Result>()
            //    .ToList();

            foreach (var env in EnvironmentQueues)
            {
                Process(env.Environment);
            }
        }

        public void Process(string environment)
        {
            var queue = EnvironmentQueues.SingleOrDefault(x => x.Environment == environment);

            if(queue == null) throw new ConDepQueueException(string.Format("Queue for environment {0} not found.", environment));

            var itemToProcess = queue.Process();
            if(itemToProcess != null) AddEvent(new EnvironmentQueueReadyForDeployment(itemToProcess.Id, itemToProcess.Environment, itemToProcess.Module, itemToProcess.Artifact));
        }

        public void Enqueue(string environment, Guid id, string module, string artifact)
        {
            var queue = GetOrCreateEnvironmentQueue(environment);
            queue.Enqueue(id, module, artifact);
            AddEvent(new DeploymentQueued(id, environment));
        }

        private EnvironmentQueue GetOrCreateEnvironmentQueue(string environment)
        {
            var queue = EnvironmentQueues.SingleOrDefault(x => x.Environment == environment);
            if (queue == null)
            {
                queue = new EnvironmentQueue(environment);
                EnvironmentQueues.Add(queue);
                //Session.Store(queue);
            }
            return queue;
        }

        public void Dequeue(string environment, Guid id)
        {
            var queue = GetOrCreateEnvironmentQueue(environment);
            queue.Dequeue(id);
            AddEvent(new DeploymentDequeued(id, environment));
        }

        public void SetInProgress(string environment, Guid id)
        {
            var queue = GetOrCreateEnvironmentQueue(environment);
            queue.SetInProgress(id);
            AddEvent(new DeploymentQueueItemInProgress(id, environment));
        }

        public void Cleanup(TimeSpan anythingOlderThan)
        {
            //var queueEnvironments = Session
            //    .Query<EnvironmentQueue_Environments.Result, EnvironmentQueue_Environments>()
            //    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
            //    .AsProjection<EnvironmentQueue_Environments.Result>()
            //    .ToList();

            foreach (var env in EnvironmentQueues)
            {
                Cleanup(env.Environment, anythingOlderThan);
            }
        }

        public void Cleanup(string environment, TimeSpan anythingOlderThan)
        {
            var queue = GetOrCreateEnvironmentQueue(environment);
            var itemsCleaned = queue.Cleanup(anythingOlderThan);

            foreach (var item in itemsCleaned)
            {
                AddEvent(new QueueItemExpired(item.Id, item.Environment, item.CreatedUtc));
            }
        }

        public IEnumerable<IEvent> Events { get { return _events; } }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void AddEvent(IEvent @event)
        {
            _events.Add(@event);
        }
    }
}