using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectInvestor : EntityBase
    {
        public string UserId { get; set; }
        public bool IsAllowed { get; set; }
        public Guid StageId { get; set; }
        public virtual Stage Stage { get; set; }
    }
}