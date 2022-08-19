using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectNewsletterSubscription : EntityBase
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public DateTimeOffset? SubscribedAt { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        
        [Encrypted]
        public string Code { get; set; }
    }
}