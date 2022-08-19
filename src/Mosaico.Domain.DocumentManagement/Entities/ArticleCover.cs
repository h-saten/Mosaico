using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class ArticleCover : DocumentBase
    {
        public Guid ArticleId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            ArticleId = id;
        }
    }
}