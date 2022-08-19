using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class TeamMember : EntityBase
    {
        public bool IsAccepted { get; set; }
        public bool IsInvitationSent { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string UserId { get; set; }
        public virtual TeamMemberRole TeamMemberRole { get; set; }
        public Guid TeamMemberRoleId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? AcceptedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        
        [Encrypted]
        public string AuthorizationCode { get; set; }
    }
}