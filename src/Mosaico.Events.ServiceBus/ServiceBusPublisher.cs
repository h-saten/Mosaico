using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Mosaico.Events.Base;
using Serilog;

namespace Mosaico.Events.ServiceBus
{
    public class ServiceBusPublisher : IEventPublisher
    {
        private readonly ILogger _logger;
        private readonly IBus _bus;
        
        public ServiceBusPublisher(IBus bus, ILogger logger = null)
        {
            _bus = bus;
            _logger = logger;
        }

        public virtual async Task SendAsync([NotNull] string path, [NotNull] CloudEvent payload, CancellationToken t)
        {
            _logger?.Verbose($"Attempting to send event {payload.Type} to {path}");
            Guid? correlationId = Guid.Parse(payload.Id);
            await _bus.Send(payload, t);
            _logger?.Verbose($"Successfully sent an event");
        }
        
        public virtual async Task PublishAsync([NotNull] string path, [NotNull] CloudEvent payload, CancellationToken t)
        {
            Guid? correlationId = Guid.Parse(payload.Id);
            _logger?.Verbose($"Attempting to send event {payload.Type} to {path} with correlation ID {correlationId}");
            var endpoint = await _bus.GetSendEndpoint(new Uri($"topic:{path}"));
            await endpoint.Send(payload, context =>
            {
                context.CorrelationId = correlationId;
            }, t);
            _logger?.Verbose($"Successfully sent an event");
        }

        public virtual Task PublishAsync(string path, CloudEvent payload)
        {
            return PublishAsync(path, payload, CancellationToken.None);
        }
        
        public virtual Task PublishAsync(CloudEvent payload)
        {
            return PublishAsync(payload.Source, payload, CancellationToken.None);
        }
    }
}