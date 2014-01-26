using System;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.QueueAggregate;

namespace ConDep.Server.Commands
{
    public class QueueDeployment : ICommand
    {
        public QueueDeployment(string module, string artifact, string environment)
        {
            Id = Guid.NewGuid();
            Module = module;
            Artifact = artifact;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Module { get; private set; }
        public string Artifact { get; private set; }
        public string Environment { get; private set; }
    }

    public class DeQueueDeployment : ICommand
    {
        public DeQueueDeployment(Guid id, string environment)
        {
            Id = id;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; set; }
    }

    public class SetDeploymentQueueItemInProgress : ICommand
    {
        public SetDeploymentQueueItemInProgress(Guid id, string environment)
        {
            Id = id;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; set; }
    }

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

    public class Deploy : ICommand
    {
        public Deploy(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }

    public class FinishDeployment : ICommand
    {
        public FinishDeployment(Guid id, string logFolder)
        {
            Id = id;
            LogFolder = logFolder;
        }

        public Guid Id { get; private set; }
        public string LogFolder { get; private set; }
    }

    public class CancelDeployment : ICommand
    {
        public Guid Id { get; private set; }
    }
}