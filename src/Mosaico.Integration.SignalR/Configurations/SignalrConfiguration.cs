using Microsoft.AspNetCore.Http.Connections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Integration.SignalR.Configurations
{
    public class SignalrConfiguration
    {
        public const string SectionName = "Signalr";
        
        public string ConnectionString { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public SignalrProviderType Provider { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpTransportType TransportType { get; set; } = HttpTransportType.LongPolling;
    }
}