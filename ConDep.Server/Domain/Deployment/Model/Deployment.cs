using System;
using System.Collections.Generic;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Domain.Deployment.Events;
using ConDep.Server.Domain.Infrastructure;
using Raven.Imports.Newtonsoft.Json;

namespace ConDep.Server.Domain.Deployment.Model
{
    public class Deployment : IAggregateRoot, IPublishEvents
    {
        private readonly List<IEvent> _events = new List<IEvent>();
        private readonly List<DeploymentMessage> _executionEvents = new List<DeploymentMessage>();
        private readonly List<DeploymentMessage> _exceptions = new List<DeploymentMessage>();

        public Deployment(Guid id, string artifact, string environment, string module)
        {
            //if(id == Guid.Empty) throw new ConDepValidationException("Id is an empty Guid!");
            Id = id;
            Artifact = artifact;
            Environment = environment;
            Module = module;
        }

        [JsonIgnore]
        public Guid? Etag { get; set; }

        public Guid Id { get; private set; }
        public string Artifact { get; private set; }
        public string Environment { get; private set; }
        public string Module { get; private set; }
        public List<DeploymentMessage> ExecutionEvents { get { return _executionEvents; } }
        public List<DeploymentMessage> Exceptions { get { return _exceptions; } }
        public DateTime StartedUtc { get; private set; }
        public DateTime FinishedUtc { get; private set; }
        public string RelativeLogLocation { get; private set; }
        public DeploymentStatus Status { get; private set; }

        public IEnumerable<IEvent> Events { get { return _events; } }
        public void ClearEvents()
        {
            _events.Clear();
        }

        public void AddEvent(IEvent @event)
        {
            _events.Add(@event);
        }

        public void Finish(DeploymentStatus status, string logFolder)
        {
            FinishedUtc = DateTime.UtcNow;
            Status = status;
            RelativeLogLocation = logFolder;
            AddEvent(new DeploymentFinished(Id, Environment));
        }

        public void Start()
        {
            StartedUtc = DateTime.UtcNow;
            Status = DeploymentStatus.InProgress;
            AddEvent(new DeploymentCreated(Id, Environment, Module, Artifact));
        }

        public void Cancel()
        {
            FinishedUtc = DateTime.UtcNow;
            Status = DeploymentStatus.Cancelled;
            AddEvent(new DeploymentCancelled(Id));
        }

        public void AddException(DateTime dateTime, Exception exception)
        {
            Exceptions.Add(new DeploymentMessage(dateTime.ToUniversalTime(), exception.Message));
        }

        public void AddException(Exception exception)
        {
            Exceptions.Add(new DeploymentMessage(DateTime.UtcNow, exception.Message));
        }

        public void AddException(TimedException exception)
        {
            Exceptions.Add(new DeploymentMessage(exception.DateTime, exception.Exception.Message));
        }

        public void AddExecutionEvent(DateTime dateTime, string message)
        {
            ExecutionEvents.Add(new DeploymentMessage(dateTime.ToUniversalTime(), message));
        }

        public void AddExecutionEvent(string message)
        {
            ExecutionEvents.Add(new DeploymentMessage(DateTime.UtcNow, message));
        }

        public void SetLogLocation(string relativeLogPath)
        {
            RelativeLogLocation = relativeLogPath;
        }
    }
}