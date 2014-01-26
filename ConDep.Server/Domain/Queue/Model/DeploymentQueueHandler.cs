using System.Threading.Tasks;
using ConDep.Server.Commands;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Infrastructure;
using Raven.Client;

namespace ConDep.Server.Domain.Queue.Model
{
    public class DeploymentQueueHandler :
        IHandleCommand<QueueDeployment>,
        IHandleCommand<DeQueueDeployment>,
        IHandleCommand<SetDeploymentQueueItemInProgress>,
        IHandleCommand<ProcessDeploymentQueue>,
        IHandleCommand<CleanupDeploymentQueue>
    {
        public DeploymentQueueHandler(IDocumentSession session)
        {
            Session = session;
        }

        private DeploymentQueue LoadDeploymentQueue()
        {
            var queue = Session.Load<DeploymentQueue>(RavenDb.GetFullId<DeploymentQueue>());
            if (queue != null) return queue;
            
            queue = new DeploymentQueue();
            Session.Store(queue);
            return queue;
        }

        public async Task<IAggregateRoot> Execute(QueueDeployment command)
        {
            var queue = LoadDeploymentQueue();
            queue.Enqueue(command.Environment, command.Id, command.Module, command.Artifact);
            return queue;
        }

        public async Task<IAggregateRoot> Execute(DeQueueDeployment command)
        {
            var queue = LoadDeploymentQueue();
            queue.Dequeue(command.Environment, command.Id);
            return queue;
        }

        public async Task<IAggregateRoot> Execute(SetDeploymentQueueItemInProgress command)
        {
            var queue = LoadDeploymentQueue();
            queue.SetInProgress(command.Environment, command.Id);
            return queue;
        }

        public async Task<IAggregateRoot> Execute(ProcessDeploymentQueue command)
        {
            var queue = LoadDeploymentQueue();
            if (string.IsNullOrWhiteSpace(command.Environment))
            {
                queue.Process();
            }
            else
            {
                queue.Process(command.Environment);
            }
            return queue;
        }

        public async Task<IAggregateRoot> Execute(CleanupDeploymentQueue command)
        {
            var queue = LoadDeploymentQueue();
            queue.Cleanup(command.AnythingOlderThan);
            return queue;
        }

        public IDocumentSession Session { get; private set; }
    }
}