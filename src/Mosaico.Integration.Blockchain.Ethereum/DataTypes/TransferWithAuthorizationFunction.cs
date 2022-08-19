using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Mosaico.Integration.Blockchain.Ethereum.DataTypes
{
    [Function("transferWithAuthorization")]
    public class TransferWithAuthorizationFunction : FunctionMessage
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
        
        [Parameter("uint8", "v", order: 7)]
        public virtual byte V { get; set; }
        
        [Parameter("bytes32", "r", order: 8)]
        public virtual byte[] R { get; set; }
        
        [Parameter("bytes32", "s", order: 9)]
        public virtual byte[] S { get; set; }
    }
}