using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.EventSourcing.Base
{
    public interface IEventRepository
    {
        Task<IReadOnlyCollection<SystemEvent>> GetEventsAsync(DateTimeOffset? from = null, DateTimeOffset? to = null, string collection = null);
        Task<IReadOnlyCollection<SystemEvent>> GetEventsAsync(string type, DateTimeOffset? from = null, DateTimeOffset? to = null, string collection = null);
        Task<SystemEvent> GetEventAsync(string id, string collection = null);
        Task<string> CreateEventAsync(SystemEvent @event, string collection = null);
    }
}