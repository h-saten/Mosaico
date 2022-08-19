using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public enum PageReviewCategory
    {
        YOUTUBE = 1,
        FACEBOOK,
        LINKEDIN
    }
    
    public class PageReview : EntityBase
    {
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
        public PageReviewCategory Category { get; set; }
        public string Link { get; set; }
        public bool IsHidden { get; set; }
    }
}