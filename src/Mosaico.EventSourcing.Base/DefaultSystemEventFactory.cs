using System;

namespace Mosaico.EventSourcing.Base
{
    public class DefaultSystemEventFactory : ISystemEventFactory
    {
        public SystemEvent Create<T>(string source, string type, T body) where T : class {
            var @event = new SystemEvent
            {
                Id = Guid.NewGuid(),
                Source = source,
                Type = type,
                CreatedAt = DateTimeOffset.UtcNow
            };
            @event.SetBody(body);
            return @event;
        }
    }
}