using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Ratings
{
    public class ProjectLike : EntityBase
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}