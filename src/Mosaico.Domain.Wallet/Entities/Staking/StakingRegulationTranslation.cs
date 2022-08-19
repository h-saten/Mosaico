using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingRegulationTranslation : TranslationBase
    {
        public virtual StakingRegulation StakingRegulation { get; set; }
    }
}