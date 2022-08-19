using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingTerms : TranslatableEntityBase<StakingTermsTranslation>
    {
        public StakingTerms()
        {
            Key = Guid.NewGuid().ToString();
            Title = Key;
        }
        
        public Guid StakingPairId { get; set; }
        public virtual StakingPair StakingPair { get; set; }
    }
}