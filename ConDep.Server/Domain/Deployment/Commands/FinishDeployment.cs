using System;
using ConDep.Server.Domain.Deployment.Model;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class FinishDeployment : ICommand
    {
        public FinishDeployment(Guid id, DeploymentStatus status, string logFolder)
        {
            Id = id;
            Status = status;
            LogFolder = logFolder;
        }

        public Guid Id { get; private set; }
        public DeploymentStatus Status { get; private set; }
        public string LogFolder { get; private set; }
    }
}