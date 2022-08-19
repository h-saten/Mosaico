using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class About : EntityBase
    {
        public Guid ContentId { get; set; }
        public virtual AboutContent Content { get; set; }
        
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}