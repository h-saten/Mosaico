using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class PageCover : DocumentBase
    {
        public Guid PageId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            PageId = id;
        }
    }
}