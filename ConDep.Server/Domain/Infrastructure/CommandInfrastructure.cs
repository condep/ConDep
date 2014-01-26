using System;
using System.Threading.Tasks;
using ConDep.Server.Domain.Infrastructure;
using Raven.Client;

namespace ConDep.Server.Infrastructure
{
    public interface ICommand
    {
        Guid Id { get; }
    }

    public interface IHandleCommand
    {
        IDocumentSession Session { get; }
    }

    public interface IHandleCommand<TCommand> : IHandleCommand
    {
        Task<IAggregateRoot> Execute(TCommand command);
    }

    public interface IHandleAsyncCommand<TCommand> : IHandleCommand
    {
        Task<IAggregateRoot> Execute(TCommand command);
    }

    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}