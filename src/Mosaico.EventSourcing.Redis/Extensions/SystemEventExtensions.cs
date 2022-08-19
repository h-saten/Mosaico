using System;
using Mosaico.EventSourcing.Base;
using Mosaico.Infrastructure.Redis.Extensions;
using StackExchange.Redis;

namespace Mosaico.EventSourcing.Redis.Extensions
{
    public static class SystemEventExtensions
    {
        public static SystemEvent ToSystemEvent(this NameValueEntry[] values)
        {
            return new SystemEvent {
                Id = Guid.Parse(values.GetValue("id")),
                Source = values.GetValue("source"),
                Type = values.GetValue("type"),
                CreatedAt = DateTimeOffset.Parse(values.GetValue("created_at")),
                BodyString = values.GetValue("body")
            };
        }
        
        public static NameValueEntry[] ToNameValueEntry(this SystemEvent @event) {
            return new NameValueEntry[]
            {
                new NameValueEntry("id", @event.Id.ToString()),
                new NameValueEntry("created_at", @event.CreatedAt.ToString()),
                new NameValueEntry("type", @event.Type),
                new NameValueEntry("source", @event.Source),
                new NameValueEntry("body", @event.BodyString)
            }; 
        }
    }
}