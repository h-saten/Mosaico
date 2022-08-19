using System.Numerics;
using Mosaico.Blockchain.Base.DAL.Events;
using Mosaico.Integration.Blockchain.Ethereum.Extensions;

namespace Mosaico.Integration.Blockchain.Ethereum.Crowdsale.Events
{
    public class TokensPurchased : IContractEvent
    {
        public static string Topic => "TokensPurchased(address,address,uint256,uint256)".Keccak256Hash();
        public static string ABI => JsonAbiFile.Read("Crowdsale/Events/TokensPurchased.json");
        
        public string Purchaser { get; set; }
        public string Beneficiary { get; set; }
        public BigInteger Value { get; set; }
        public BigInteger Amount { get; set; }
    }
}