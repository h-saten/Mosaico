using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Mosaico.Events.Base;
using Mosaico.Events.RabbitMq;
using Mosaico.EventSourcing.Base;
using Serilog;

namespace Mosaico.Core
{
    public class DurableRabbitMqEventPublisher : RabbitMQPublisher
    {
        private readonly ISystemEventFactory _systemEventFactory;
        private readonly IEventRepository _eventRepository;
        
        public DurableRabbitMqEventPublisher(IBus provider, ISystemEventFactory systemEventFactory, IEventRepository eventRepository, ILogger logger = null) : base(provider, logger)
        {
            _systemEventFactory = systemEventFactory;
            _eventRepository = eventRepository;
        }
        
        public override async Task PublishAsync(string topic, CloudEvent payload, CancellationToken t)
        {
            var systemEvent = _systemEventFactory.Create(payload.Source, payload.Type, payload);
            await _eventRepository.CreateEventAsync(systemEvent);
            await base.PublishAsync(topic, payload, t);
        }
    }
}