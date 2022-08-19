using System.Text.Json.Serialization;
using Mosaico.Blockchain.Base.DAL.Events;

namespace Mosaico.Integration.Blockchain.Moralis.ApiClients.Models
{
    public class MoralisContractEvent<TEvent> where TEvent : class, IContractEvent
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
        
        [JsonPropertyName("data")]
        public TEvent Data { get; set; }
    }
}