using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.DataTypes
{
    [Struct("TransferWithAuthorization")]
    public class TransferWithAuthorization
    {
        [Parameter("address", "from", order: 1)]
        public virtual string From { get; set; }
        
        [Parameter("address", "to", order: 2)]
        public virtual string To { get; set; }
        
        [Parameter("uint256", "value", order: 3)]
        public virtual BigInteger Value { get; set; }
        
        [Parameter("uint256", "validAfter", order: 4)]
        public virtual BigInteger ValidAfter { get; set; }
        
        [Parameter("uint256", "validBefore", order: 5)]
        public virtual BigInteger ValidBefore { get; set; }
        
        [Parameter("bytes32", "nonce", order: 6)]
        public virtual byte[] Nonce { get; set; }
    }
}