using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition
{
    public partial class StakingSettings : StakingSettingsBase { }

    public class StakingSettingsBase 
    {
        [Parameter("uint256", "maxRewardPerStaker", 1)]
        public virtual BigInteger MaxRewardPerStaker { get; set; }
        [Parameter("uint256", "rewardCycle", 2)]
        public virtual BigInteger RewardCycle { get; set; }
        [Parameter("address", "stakingToken", 3)]
        public virtual string StakingToken { get; set; }
        [Parameter("uint256", "minimumStakingAmount", 4)]
        public virtual BigInteger MinimumStakingAmount { get; set; }
        [Parameter("address", "tmos", 5)]
        public virtual string Tmos { get; set; }
        [Parameter("uint256", "tokendaId", 6)]
        public virtual BigInteger TokendaId { get; set; }
    }
}
