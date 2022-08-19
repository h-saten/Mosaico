using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingRegulation : TranslatableEntityBase<StakingRegulationTranslation>
    {
        public StakingRegulation()
        {
            Key = Guid.NewGuid().ToString();
            Title = Key;
        }
        
        public Guid StakingPairId { get; set; }
        public virtual StakingPair StakingPair { get; set; }
    }
}