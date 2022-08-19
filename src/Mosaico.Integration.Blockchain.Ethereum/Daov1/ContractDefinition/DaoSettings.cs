using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition
{
    public partial class DaoSettings : DaoSettingsBase { }

    public class DaoSettingsBase 
    {
        [Parameter("string", "name", 1)]
        public virtual string Name { get; set; }
        [Parameter("bool", "isVotingEnabled", 2)]
        public virtual bool IsVotingEnabled { get; set; }
        [Parameter("bool", "onlyOwnerProposals", 3)]
        public virtual bool OnlyOwnerProposals { get; set; }
        [Parameter("uint256", "initialVotingDelay", 4)]
        public virtual BigInteger InitialVotingDelay { get; set; }
        [Parameter("uint256", "initialVotingPeriod", 5)]
        public virtual BigInteger InitialVotingPeriod { get; set; }
        [Parameter("uint256", "quorum", 6)]
        public virtual BigInteger Quorum { get; set; }
        [Parameter("address", "owner", 7)]
        public virtual string Owner { get; set; }
    }
}
