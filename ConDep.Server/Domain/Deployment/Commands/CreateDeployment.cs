using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class CreateDeployment : ICommand
    {
        public CreateDeployment(Guid id, string environment, string module, string artifact)
        {
            Id = id;
            Environment = environment;
            Module = module;
            Artifact = artifact;
        }

        public Guid Id { get; private set; }
        public string Environment { get; private set; }
        public string Artifact { get; private set; }
        public string Module { get; private set; }
    }
}