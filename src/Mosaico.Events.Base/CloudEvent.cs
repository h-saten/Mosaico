using System;
using Newtonsoft.Json;

namespace Mosaico.Events.Base
{
    public class CloudEvent
    {
        [JsonProperty("specversion")] 
        public string SpecVersion { get; set; }

        [JsonProperty("source")] 
        public string Source { get; set; }

        [JsonProperty("type")] 
        public string Type { get; set; }

        [JsonProperty("subject")] 
        public string Subject { get; set; }

        [JsonProperty("time")] 
        public DateTimeOffset Time { get; set; }

        [JsonProperty("datacontenttype")] 
        public string DataContentType { get; set; }

        [JsonProperty("data")] 
        public string Data { get; set; }

        [JsonProperty("id")] 
        public string Id { get; set; }

        public void SetData<TPayload>(TPayload payload) where TPayload : class
        {
            Data = payload == null ? string.Empty : JsonConvert.SerializeObject(payload);
        }

        public TPayload GetData<TPayload>() where TPayload : class
        {
            return string.IsNullOrWhiteSpace(Data) ? null : JsonConvert.DeserializeObject<TPayload>(Data);
        }
    }
}