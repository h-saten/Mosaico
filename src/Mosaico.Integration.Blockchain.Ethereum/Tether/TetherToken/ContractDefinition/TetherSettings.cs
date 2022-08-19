using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken.ContractDefinition
{
    public class TetherSettings 
    {
        [Parameter("uint", "_initialSupply", 1)]
        public virtual BigInteger InitialSupply { get; set; }
        
        [Parameter("string", "_name", 2)]
        public virtual string Name { get; set; }
        
        [Parameter("string", "_symbol", 3)]
        public virtual string Symbol { get; set; }
        
        [Parameter("uint", "_decimals", 4)]
        public virtual BigInteger Decimals { get; set; }
    }
}
