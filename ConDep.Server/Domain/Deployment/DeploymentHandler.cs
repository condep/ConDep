using System.Threading.Tasks;
using ConDep.Server.Commands;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.DeploymentAggregate;
using Raven.Client;

namespace ConDep.Server.DomainEvents
{
    public class DeploymentHandler : 
        IHandleCommand<CreateDeployment>,
        IHandleCommand<FinishDeployment>
    {
        public DeploymentHandler(IDocumentSession session)
        {
            Session = session;
        }

        public async Task<IAggregateRoot> Execute(CreateDeployment command)
        {
            var deployment = new Deployment(command.Id, command.Artifact, command.Environment, command.Module);
            Session.Store(deployment);
            deployment.Start();
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(FinishDeployment command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            deployment.Finish(DeploymentStatus.Success, command.LogFolder);
            return deployment;
        }

        public IDocumentSession Session { get; private set; }
    }
}