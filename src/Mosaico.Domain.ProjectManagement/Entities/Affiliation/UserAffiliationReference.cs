using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Affiliation
{
    public class UserAffiliationReference : EntityBase
    {
        public string ReferencedUserId { get; set; }
        public Guid UserAffiliationId { get; set; }
        public virtual UserAffiliation UserAffiliation { get; set; }
        public Guid? ProjectId { get; set; }
    }
}