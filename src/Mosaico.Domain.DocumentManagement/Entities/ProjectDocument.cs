using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class ProjectDocument : DocumentBase
    {
        public Guid ProjectId { get; set; }
        public bool IsMandatory { get; set; }
        public override void SetRelatedEntityId(Guid id)
        {
            ProjectId = id;
        }
    }
}
