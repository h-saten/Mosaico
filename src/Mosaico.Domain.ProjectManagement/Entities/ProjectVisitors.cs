using Mosaico.Domain.Base;
using System;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectVisitors : EntityBase
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
