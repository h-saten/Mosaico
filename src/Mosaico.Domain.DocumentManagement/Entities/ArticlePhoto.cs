using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class ArticlePhoto : DocumentBase
    {
        public Guid ArticleId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            ArticleId = id;
        }
    }
}