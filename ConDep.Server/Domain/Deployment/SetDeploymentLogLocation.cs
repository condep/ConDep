using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment
{
    public class SetDeploymentLogLocation : ICommand
    {
        public SetDeploymentLogLocation(Guid id, string relativeLogPath)
        {
            Id = id;
            RelativeLogPath = relativeLogPath;
        }

        public Guid Id { get; private set; }
        public string RelativeLogPath { get; private set; }
    }
}