using System.Text.Json.Serialization;

namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class NativeBalance
    {
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }
}