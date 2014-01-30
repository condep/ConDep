using System;
using System.Threading.Tasks;
using ConDep.Server.Domain.Deployment;
using ConDep.Server.Domain.Deployment.Commands;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Domain.Queue.Commands;
using ConDep.Server.Domain.Queue.Model;
using Raven.Client;
using System.Linq;

namespace ConDep.Server.Domain.Queue
{
    public class DeploymentQueueHandler :
        IHandleCommand<QueueDeployment>,
        IHandleCommand<DeQueueDeployment>,
        IHandleCommand<SetDeploymentQueueItemInProgress>,
        IHandleCommand<ProcessEnvironmentQueue>,
        IHandleCommand<ProcessDeploymentQueue>,
        IHandleCommand<MarkQueueItemReadyForDeployment>,
        IHandleCommand<CleanupDeploymentQueue>
    {
        private readonly ICommandBus _cmdBus;

        public DeploymentQueueHandler(IDocumentSession session, ICommandBus cmdBus)
        {
            _cmdBus = cmdBus;
            Session = session;
        }

        //private DeploymentQueue LoadDeploymentQueue()
        //{
        //    var queue = Session.Load<DeploymentQueue>(RavenDb.GetFullId<DeploymentQueue>());
        //    if (queue != null) return queue;
            
        //    queue = new DeploymentQueue();
        //    Session.Store(queue);
        //    return queue;
        //}

        private QueueItem LoadQueueItem(string environment, Guid id)
        {
            return Session.Load<QueueItem>(environment + "/" + id);
        }

        public async Task<IAggregateRoot> Execute(QueueDeployment command)
        {
            var item = new QueueItem(command.Environment, command.Module, command.Artifact);
            Session.Store(item);
            return item;
        }

        public async Task<IAggregateRoot> Execute(DeQueueDeployment command)
        {
            var item = LoadQueueItem(command.Environment, command.Id);
            Session.Delete(item);
            return item;
        }

        public async Task<IAggregateRoot> Execute(SetDeploymentQueueItemInProgress command)
        {
            var item = LoadQueueItem(command.Environment, command.Id);
            item.SetInProgress();
            return item;
        }

        public async Task<IAggregateRoot> Execute(ProcessEnvironmentQueue command)
        {
            var first = Session
                .Query<QueueItem, QueueItem_FirstInAllEnvironments>()
                .Where(x => x.Environment == command.Environment).AsProjection<QueueItem>().SingleOrDefault();
 
            if (first != null && first.QueueStatus == QueueStatus.Waiting)
            {
                first.SetReadyForDeployment();
            }
            return first;
        }

        public async Task<IAggregateRoot> Execute(MarkQueueItemReadyForDeployment command)
        {
            var item = LoadQueueItem(command.Environment, command.Id);
            if (item != null && item.QueueStatus == QueueStatus.Waiting)
            {
                item.SetReadyForDeployment();
            }
            return item;
        }

        public Task<IAggregateRoot> Execute(ProcessDeploymentQueue command)
        {
            var firstInAllQueues = Session
                .Query<QueueItem, QueueItem_FirstInAllEnvironments>();

            if (firstInAllQueues != null)
                foreach (var item in firstInAllQueues)
                {
                    var cmd = new MarkQueueItemReadyForDeployment(item.DeploymentId, item.Environment);
                    _cmdBus.Send(cmd);
                }
            return Task.FromResult<IAggregateRoot>(null);
        }


        public async Task<IAggregateRoot> Execute(CleanupDeploymentQueue command)
        {
            return null;
            //var queue = LoadDeploymentQueue();
            //queue.Cleanup(command.AnythingOlderThan);
            //return queue;
        }

        public IDocumentSession Session { get; private set; }
    }
}