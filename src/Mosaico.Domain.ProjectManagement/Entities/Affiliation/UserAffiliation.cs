using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Affiliation
{
    public class UserAffiliation : EntityBase
    {
        public string UserId { get; set; }
        public string AccessCode { get; set; }
        public virtual List<Partner> PartnerAssignments { get; set; } = new List<Partner>();
    }
}