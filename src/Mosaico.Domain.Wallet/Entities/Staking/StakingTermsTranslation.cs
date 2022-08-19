using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingTermsTranslation : TranslationBase
    {
        public virtual StakingTerms StakingTerms { get; set; }
    }
}