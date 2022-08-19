using System;
using Newtonsoft.Json;

namespace Mosaico.EventSourcing.Base
{
    public class SystemEvent
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Type { get; set; }

        public void SetBody<T>(T value) where T : class {
            BodyString = value == null ? string.Empty : JsonConvert.SerializeObject(value);
        }

        public T GetBody<T>() where T : class
        {
            return string.IsNullOrWhiteSpace(BodyString) ? null : JsonConvert.DeserializeObject<T>(BodyString);
        }

        public string BodyString { get; set; }
    }
}