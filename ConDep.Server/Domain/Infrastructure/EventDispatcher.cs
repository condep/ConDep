using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;

namespace ConDep.Server.Domain.Infrastructure
{
    public class EventDispatcher
    {
        private static readonly IDictionary<string, IHandleEvent> _handlers = new Dictionary<string, IHandleEvent>();
        private static readonly IDictionary<string, IHandleEvent> _handlerByEvents = new Dictionary<string, IHandleEvent>();

        public static void AutoRegister()
        {
            var handlerTypes = typeof(IHandleEvent).Assembly.GetTypes().Where(t => typeof (IHandleEvent).IsAssignableFrom(t) && t.IsClass);
            foreach (var handlerType in handlerTypes)
            {
                try
                {
                    var handler = ObjectFactory.GetInstance(handlerType) as IHandleEvent;

                    Regiser(handler);
                }
                catch (Exception ex)
                {
                    throw new ConDepUnableToRegisterEventHandlerException("See inner exception for details.", ex);
                }
            }
        }

        public static void Regiser(IHandleEvent handler)
        {
            var handlerType = handler.GetType();
            var handlerId = handlerType.FullName;

            if (_handlers.ContainsKey(handlerId))
            {
                throw new ConDepEventHandlerAllreadyRegisteredException();
            }

            _handlers.Add(handlerId, handler);

            var eventTypes = GetEventTypesForHandler(handler);
            foreach (var eventType in eventTypes)
            {
                _handlerByEvents.Add(eventType.FullName, handler);
            }
        }

        private static IEnumerable<Type> GetEventTypesForHandler(IHandleEvent handler)
        {
            var handlerType = handler.GetType();
            var interfaces = handlerType.GetInterfaces();

            return interfaces
                .Where(@interface => @interface.IsGenericType)
                .SelectMany(@interface => @interface.GenericTypeArguments
                    .Where(eventType => typeof (IEvent).IsAssignableFrom(eventType)));
        }

        public static Task Dispatch(IEvent @event)
        {
            var key = @event.GetType().FullName;
            if(!_handlerByEvents.ContainsKey(key))
                return Task.FromResult<object>(null);

            var handler = _handlerByEvents[key];
            var methodInfo = handler.GetType().GetMethod("Handle", new[] {@event.GetType()});
            return Task.Run(() =>
                {
                    methodInfo.Invoke(handler, new object[] { @event });
                });
        }
    }
}