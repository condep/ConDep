using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Server.Commands;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.DeploymentAggregate;
using Raven.Client;

namespace ConDep.Server.DomainEvents
{
    public class DeploymentHandler :
        IHandleCommand<CreateDeployment>,
        IHandleCommand<Deploy>,
        IHandleCommand<AddDeploymentExecutionEvent>,
        IHandleCommand<AddDeploymentException>,
        IHandleCommand<AddDeploymentTimedException>,
        IHandleCommand<CancelDeployment>,
        IHandleCommand<FinishDeployment>,
        IHandleCommand<SetDeploymentLogLocation>
    {
        private readonly DeploymentService _deploymentService;

        public DeploymentHandler(IDocumentSession session, DeploymentService deploymentService)
        {
            _deploymentService = deploymentService;
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
            deployment.Finish(command.Status, command.LogFolder);
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(Deploy command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            var config = Session.Load<ConDepEnvConfig>(RavenDb.GetFullId<ConDepEnvConfig>(deployment.Environment));
            var execData = new ExecutionData
                {
                    DeploymentId = deployment.Id,
                    Artifact = deployment.Artifact,
                    Environment = deployment.Environment,
                    Module = deployment.Module
                };

            _deploymentService.Deploy(execData, config);
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(AddDeploymentExecutionEvent command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            deployment.AddExecutionEvent(command.Message);
            return deployment;
        }

        //Todo: Need to handle scoping of token sources for this to work
        public async Task<IAggregateRoot> Execute(CancelDeployment command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            _deploymentService.Cancel(deployment.Id);
            deployment.Cancel();
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(AddDeploymentException command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            deployment.AddException(command.Exception);
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(AddDeploymentTimedException command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            deployment.AddException(command.TimedException);
            return deployment;
        }

        public async Task<IAggregateRoot> Execute(SetDeploymentLogLocation command)
        {
            var deployment = Session.Load<Deployment>(command.Id);
            deployment.SetLogLocation(command.RelativeLogPath);
            return deployment;
        }

        public IDocumentSession Session { get; private set; }
    }
}