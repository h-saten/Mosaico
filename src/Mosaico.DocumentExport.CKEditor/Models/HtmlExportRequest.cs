using Newtonsoft.Json;

namespace Mosaico.DocumentExport.CKEditor.Models
{
    public class HtmlExportRequest
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        
        [JsonProperty("css")]
        public string Css { get; set; }
    }
}