using System.Text.Json.Serialization;

namespace Mosaico.Integration.Blockchain.Moralis.ApiClients.Models
{
    public class MoralisERC20Transfer
    {
        [JsonPropertyName("transaction_hash")]
        public string TransactionHash { get; set; }
        
        [JsonPropertyName("address")]
        public string Address { get; set; }
        
        [JsonPropertyName("block_timestamp")]
        public string BlockTimestamp { get; set; }
        
        [JsonPropertyName("block_number")]
        public string BlockNumber { get; set; }
        
        [JsonPropertyName("block_hash")]
        public string BlockHash { get; set; }
        
        [JsonPropertyName("to_address")]
        public string ToAddress { get; set; }
        
        [JsonPropertyName("from_address")]
        public string FromAddress { get; set; }
        
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}