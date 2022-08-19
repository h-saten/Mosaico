using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class InvestmentPackageLogo : DocumentBase
    {
        public Guid PageId { get; set; }
        public Guid InvestmentPackageId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            PageId = id;
        }
    }
}