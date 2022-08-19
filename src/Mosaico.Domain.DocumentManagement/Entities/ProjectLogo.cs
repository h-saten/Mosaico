using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class ProjectLogo : DocumentBase
    {
        public Guid ProjectId { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            ProjectId = id;
        }
    }
}