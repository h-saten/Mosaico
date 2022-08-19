using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class CompanyDocument : DocumentBase
    {
        public Guid CompanyId { get; set; }
        public bool IsMandatory { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            CompanyId = id;
        }
    }
}
