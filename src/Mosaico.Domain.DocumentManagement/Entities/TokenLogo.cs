using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class TokenLogo : DocumentBase
    {
        public Guid TokenId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            TokenId = id;
        }
    }
}