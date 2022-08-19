using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mosaico.Integration.Blockchain.Moralis.ApiClients.Models
{
    public class Erc20TransfersResponse
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        
        [JsonPropertyName("result")]
        public List<MoralisERC20Transfer> Result { get; set; }
    }
}