using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectMember : EntityBase
    {
        // Optional ID of the associated user. Will be filled in when user accepts invitation
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual ProjectRole Role { get; set; }
        public bool IsInvitationSent { get; set; }
        // deadline by when user must accept invitation
        public DateTimeOffset ExpiresAt { get; set; }
        // time when user accepted invitation
        public DateTimeOffset? AcceptedAt { get; set; }
        // if user accepted invitation
        public bool IsAccepted { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public string Email { get; set; }
        
        [Encrypted]
        public string AuthorizationCode { get; set; }
    }
}