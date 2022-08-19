using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition
{
    public partial class ERC20Settings : ERC20SettingsBase { }

    public class ERC20SettingsBase 
    {
        [Parameter("bool", "isMintable", 1)]
        public virtual bool IsMintable { get; set; }
        [Parameter("bool", "isBurnable", 2)]
        public virtual bool IsBurnable { get; set; }
        [Parameter("string", "name", 3)]
        public virtual string Name { get; set; }
        [Parameter("string", "symbol", 4)]
        public virtual string Symbol { get; set; }
        [Parameter("uint256", "initialSupply", 5)]
        public virtual BigInteger InitialSupply { get; set; }
        [Parameter("address", "walletAddress", 6)]
        public virtual string WalletAddress { get; set; }
        [Parameter("bool", "isGovernance", 7)]
        public virtual bool IsGovernance { get; set; }
    }
}
