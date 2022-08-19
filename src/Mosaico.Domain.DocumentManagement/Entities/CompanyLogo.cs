using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class CompanyLogo : DocumentBase
    {
        public Guid CompanyId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            CompanyId = id;
        }
    }
}