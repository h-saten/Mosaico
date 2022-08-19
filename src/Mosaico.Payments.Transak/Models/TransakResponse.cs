using Newtonsoft.Json;

namespace Mosaico.Payments.Transak.Models
{
    public class TransakResponse<TBody>
    {
        [JsonProperty("response")]
        public TBody Response { get; set; }
    }
}