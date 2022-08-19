using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.EventSourcing.Base;
using Mosaico.EventSourcing.Redis.Configurations;
using Mosaico.EventSourcing.Redis.Extensions;
using Mosaico.Infrastructure.Redis;
using Mosaico.Infrastructure.Redis.Exceptions;
using Serilog;
using StackExchange.Redis;

namespace Mosaico.EventSourcing.Redis
{
    public class RedisEventRepository : IEventRepository
    {
        private readonly RedisEventSourcingConfiguration _config;
        private readonly IValidator<RedisEventSourcingConfiguration> _configValidator;
        private readonly ILogger _logger;
        public RedisEventRepository(RedisEventSourcingConfiguration config, ILogger logger = null, IValidator<RedisEventSourcingConfiguration> configValidator = null)
        {
            _config = config;
            _logger = logger;
            _configValidator = configValidator;
        }

        private IConnectionMultiplexer GetConnection() {
            if (_configValidator != null)
            {
                var result = _configValidator.Validate(_config);
                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault()?.ErrorMessage ?? "Redis is misconfigured";
                    _logger?.Error(error);
                    throw new ValidationException(error);
                }
            }
            return ConnectionMultiplexer.Connect(_config.RedisConnectionString);
        }

        protected async Task<IReadOnlyCollection<SystemEvent>> GetEventsByPredicateAsync(Predicate<SystemEvent> predicate, string collection = null)
        {
            //TODO: check performance and improve if needed
            var @events = new List<SystemEvent>();
            var connection = GetConnection();
            var db = connection.GetDatabase(_config.RedisDatabase);
            var stream = string.IsNullOrWhiteSpace(collection) ? _config.StreamName : collection;
            _logger?.Verbose($"searching events by a predicate in {stream}");
            var eventEntries = await db.StreamReadAsync(stream, "0-0", Constants.DefaultPageSize);
            while (eventEntries != null && eventEntries.Length != 0)
            {
                foreach (var eventEntry in eventEntries)
                {
                    var systemEvent = eventEntry.Values.ToSystemEvent();
                    if (predicate(systemEvent))
                    {
                        @events.Add(systemEvent);
                    }
                }
                eventEntries = await db.StreamReadAsync(stream, eventEntries.Last().Id, Constants.DefaultPageSize);
            }
            _logger?.Verbose($"Found {@events.Count} events");
            return @events.AsReadOnly();
            
        }

        public Task<IReadOnlyCollection<SystemEvent>> GetEventsAsync(DateTimeOffset? @from = null, DateTimeOffset? to = null, string collection = null)
        {
            return GetEventsByPredicateAsync(e =>
                from.HasValue && e.CreatedAt >= from && !to.HasValue ||
                to.HasValue && e.CreatedAt <= to && !from.HasValue ||
                to.HasValue && from.HasValue && e.CreatedAt >= from && e.CreatedAt <= to ||
                !to.HasValue && !from.HasValue, collection);
        }

        public Task<IReadOnlyCollection<SystemEvent>> GetEventsAsync(string type, DateTimeOffset? @from = null, DateTimeOffset? to = null, string collection = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new InvalidRedisQueryParameterException(nameof(type));
            }
            return GetEventsByPredicateAsync(e =>
                from.HasValue && e.CreatedAt >= from && !to.HasValue && e.Type == type ||
                to.HasValue && e.CreatedAt <= to && !from.HasValue && e.Type == type ||
                to.HasValue && from.HasValue && e.CreatedAt >= from && e.CreatedAt <= to && e.Type == type ||
                !to.HasValue && !from.HasValue && e.Type == type, collection);
        }

        public async Task<SystemEvent> GetEventAsync(string id, string collection = null)
        {
            var connection = GetConnection();
            var db = connection.GetDatabase(_config.RedisDatabase);
            SystemEvent @event = default;
            var stream = string.IsNullOrWhiteSpace(collection) ? _config.StreamName : collection;
            _logger?.Verbose($"Looking for event in {stream} via id {id}");
            var eventEntries = await db.StreamReadAsync(stream, "0-0", Constants.DefaultPageSize);
            while (eventEntries != null && eventEntries.Length != 0)
            {
                var eventEntry = eventEntries.FirstOrDefault(e => e.Id == id || e["id"] == id);
                if (!eventEntry.IsNull)
                {
                    @event = eventEntry.Values.ToSystemEvent();
                    break;
                }
                eventEntries = await db.StreamReadAsync(stream, eventEntries.Last().Id, Constants.DefaultPageSize);
            }
            _logger?.Verbose($"Event {@event.Source}/{@event.Type} was found");
            return @event;
        }

        public async Task<string> CreateEventAsync(SystemEvent @event, string collection = null)
        {
            if(@event == null){
                throw new InvalidRedisQueryParameterException(nameof(@event));    
            }
            
            @event.Id = @event.Id == Guid.Empty ? Guid.NewGuid() : @event.Id;

            var connection = GetConnection();
            var db = connection.GetDatabase(_config.RedisDatabase);
            var values = @event.ToNameValueEntry();
            var stream = string.IsNullOrWhiteSpace(collection) ? _config.StreamName : collection;
            _logger?.Verbose($"Creating event {@event.Source}/{@event.Type} in {stream}");
            string messageId = await db.StreamAddAsync(stream, values);
            _logger?.Verbose($"Document ID is {messageId}");
            return messageId;
        }
    }
}