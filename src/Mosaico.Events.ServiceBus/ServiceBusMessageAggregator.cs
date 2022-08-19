using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using Mosaico.Events.Base;
using Serilog;

namespace Mosaico.Events.ServiceBus
{
    public class ServiceBusMessageAggregator : IConsumer<CloudEvent>
    {
        private readonly IEnumerable<IEventHandler> _eventHandlers;
        private readonly ILogger _logger;

        public ServiceBusMessageAggregator(ILogger logger = null, IEnumerable<IEventHandler> eventHandlers = null)
        {
            _logger = logger;
            _eventHandlers = eventHandlers;
        }

        public async Task Consume(ConsumeContext<CloudEvent> context)
        {
            if (context.Message == null)
            {
                _logger?.Warning($"Received a message from the event bus with empty payload");
                return;
            }
            
            _logger?.Verbose($"Received event with correlation Id {context.CorrelationId} of type {context.Message?.Type} from {context.Message?.Source}");
            foreach (var eventHandler in _eventHandlers)
            {
                var attribute = eventHandler.GetType().GetCustomAttribute<EventTypeFilterAttribute>();
                if (attribute == null || (attribute?.Type != null && context.Message.Type == attribute.Type.FullName))
                {
                    _logger?.Verbose($"Attempting to run the message {context.CorrelationId} through the handler {eventHandler.GetType().FullName}");
                    try
                    {
                        await eventHandler.HandleAsync(context.Message).ContinueWith(async (task) =>
                        {
                            if (task.IsCompleted)
                            {
                                await LogSuccessAsync(eventHandler.GetType().FullName, context.CorrelationId?.ToString());
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger?.Warning($"Exception was thrown during execution of {eventHandler.GetType().FullName}: {ex.Message} / {ex.StackTrace}");
                    }
                }
            }
        }

        private Task LogSuccessAsync(string handlerName, string correlationId)
        {
            _logger?.Verbose($"Successfully executed {handlerName} for the message {correlationId}");
            return Task.CompletedTask;
        }
    }
}