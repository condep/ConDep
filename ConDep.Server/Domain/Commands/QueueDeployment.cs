using System;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.DeploymentAggregate;
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

    public class CancelDeployment : ICommand
    {
        public CancelDeployment(Guid id)
        {
            Id = id;        
        }

        public Guid Id { get; private set; }
    }

    public class AddDeploymentExecutionEvent : ICommand
    {
        public AddDeploymentExecutionEvent(Guid id, string message)
        {
            Id = id;
            Message = message;
        }

        public Guid Id { get; private set; }
        public string Message { get; private set; }
    }

    public class  AddDeploymentException : ICommand
    {
        public AddDeploymentException(Guid id, Exception ex)
        {
            Id = id;
            Exception = ex;
        }

        public Guid Id { get; private set; }
        public Exception Exception { get; private set; }
    }

    public class AddDeploymentTimedException : ICommand
    {
        public AddDeploymentTimedException(Guid id, TimedException ex)
        {
            Id = id;
            TimedException = ex;
        }

        public Guid Id { get; private set; }
        public TimedException TimedException { get; private set; }
    }

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