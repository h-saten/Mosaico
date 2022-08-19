using Newtonsoft.Json;

namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class TokenDetails
    {
        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }
        [JsonProperty("tokenName")]
        public string TokenName { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("divisor")]
        public int Divisor { get; set; }
        [JsonProperty("tokenType")]
        public string TokenType { get; set; }
        [JsonProperty("totalSupply")]
        public decimal TotalSupply { get; set; }
        public bool Mintable { get; set; }
        public bool Burnable { get; set; }
    }
}