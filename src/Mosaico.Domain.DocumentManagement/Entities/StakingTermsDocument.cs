using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class StakingTermsDocument : DocumentBase
    {
        public Guid StakingPairId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            StakingPairId = id;
        }
    }
}