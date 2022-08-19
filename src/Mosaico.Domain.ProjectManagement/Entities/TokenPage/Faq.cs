using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class Faq : EntityBase
    {
        public bool IsHidden { get; set; }
        public int Order { get; set; }
        
        public Guid TitleId { get; set; }
        public virtual FaqTitle Title { get; set; }
        
        public Guid ContentId { get; set; }
        public virtual FaqContent Content { get; set; }
        
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}