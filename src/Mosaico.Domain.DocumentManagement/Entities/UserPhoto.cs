using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class UserPhoto : DocumentBase
    {
        public Guid UserId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            UserId = id;
        }
    }
}