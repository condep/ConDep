using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Server.Model.QueueAggregate;

namespace ConDep.Server.Domain.Queue.Model
{
    public class EnvironmentQueue
    {
        protected EnvironmentQueue()
        {
            Queue = new List<QueueItem>();
        }

        public EnvironmentQueue(string environment)
        {
            Queue = new List<QueueItem>();
            Environment = environment;
        }

        protected List<QueueItem> Queue
        {
            get; private set;
        }

        public string Environment { get; private set; }

        public void Enqueue(Guid id, string module, string artifact)
        {
            Queue.Add(new QueueItem(id, Environment, module, artifact));
        }

        public void Dequeue(Guid id)
        {
            var item = Queue.SingleOrDefault(x => x.Id == id);
            if(item == null) throw new ConDepQueueItemDoesNotExistException();
            Queue.Remove(item);
        }

        public QueueItem Process()
        {
            if (Queue.Count == 0) return null;

            var item = Peek();

            if (item.QueueStatus == QueueStatus.Waiting)
            {
                item.QueueStatus = QueueStatus.ReadyForDeployment;
                return item;
            }
            return null;
        }

        public void SetInProgress(Guid id)
        {
            var item = Queue.SingleOrDefault(x => x.Id == id);
            if (item == null) throw new ConDepQueueItemDoesNotExistException();

            item.QueueStatus = QueueStatus.DeploymentInProgress;
        }

        public IEnumerable<QueueItem> Cleanup(TimeSpan anythingOlderThan)
        {
            var itemsToCleanUp = Queue.Where(item =>
                    item.QueueStatus == QueueStatus.Waiting &&
                    DateTime.UtcNow.Subtract(anythingOlderThan) > item.CreatedUtc
                ).ToList();

            foreach (var item in itemsToCleanUp)
            {
                Queue.Remove(item);
            }
            return itemsToCleanUp;
        }

        public int Count
        {
            get { return Queue.Count; }
        }

        private QueueItem Peek()
        {
            return Queue.First();
        }

    }
}