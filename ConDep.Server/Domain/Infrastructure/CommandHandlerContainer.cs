using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace ConDep.Server.Domain.Infrastructure
{
    public class CommandHandlerContainer
    {
        public static IHandleCommand<TCommand> Resolve<TCommand>() where TCommand : ICommand
        {
            var type = typeof (TCommand);
            return FindHandler<TCommand>(type);
        }

        private static IHandleCommand<TCommand> FindHandler<TCommand>(Type type) where TCommand : ICommand
        {
            var handlerTypes = type.Assembly.GetTypes()
                                   .Where(t => typeof (IHandleCommand<TCommand>).IsAssignableFrom(t)).ToList();

            ValidateExactlyOneCommandHandler(type, handlerTypes);

            var handler = CreateInstanceOfCommandHandler<TCommand>(handlerTypes);
            return handler;
        }

        private static void ValidateExactlyOneCommandHandler(Type type, List<Type> handlerTypes)
        {
            if (handlerTypes.Count() > 1)
                throw new ConDepCommandHandlerException(
                    "More than one command handler is registered to handle the command " + type.FullName);

            if (!handlerTypes.Any())
                throw new ConDepCommandHandlerException("No command handler is registered to handle the command " +
                                                        type.FullName);
        }

        private static IHandleCommand<TCommand> CreateInstanceOfCommandHandler<TCommand>(IEnumerable<Type> handlerTypes) where TCommand : ICommand
        {
            var handlerType = handlerTypes.Single();
            var handler = ObjectFactory.GetInstance(handlerType) as IHandleCommand<TCommand>;
            return handler;
        }
    }
}