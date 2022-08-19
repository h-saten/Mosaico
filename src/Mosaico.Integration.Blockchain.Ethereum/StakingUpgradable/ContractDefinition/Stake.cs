using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition
{
    public partial class Stake : StakeBase { }

    public class StakeBase 
    {
        [Parameter("address", "staker", 1)]
        public virtual string Staker { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "since", 3)]
        public virtual BigInteger Since { get; set; }
        [Parameter("address", "token", 4)]
        public virtual string Token { get; set; }
        [Parameter("bool", "active", 5)]
        public virtual bool Active { get; set; }
    }
}
