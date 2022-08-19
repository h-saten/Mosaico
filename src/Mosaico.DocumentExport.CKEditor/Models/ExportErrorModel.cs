using Newtonsoft.Json;

namespace Mosaico.DocumentExport.CKEditor.Models
{
    public class ExportErrorModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }
        
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
    }
}