using System;
using Mosaico.Events.Base.Exceptions;

namespace Mosaico.Events.Base
{
    public class CloudEventFactory : IEventFactory
    {
        public CloudEvent CreateEvent<TPayload>(string source, TPayload payload) where TPayload : class
        {
            if (payload == null) 
                throw new InvalidEventPayloadException("Payload is null");

            var @event = new CloudEvent
            {
                Id = Guid.NewGuid().ToString(),
                SpecVersion = Constants.SpecVersions.v1,
                Time = DateTimeOffset.UtcNow,
                DataContentType = Constants.ContentTypes.JSON,
                Subject = typeof(TPayload).Name,
                Type = typeof(TPayload).FullName,
                Source = source
            };
            @event.SetData(payload);
            return @event;
        }
    }
}