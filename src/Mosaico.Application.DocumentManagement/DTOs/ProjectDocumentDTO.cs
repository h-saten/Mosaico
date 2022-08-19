using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.DTOs
{
    public class ProjectDocumentDTO : DocumentDTO
    {
        public Guid ProjectId { get; set; }
        public bool IsMandatory { get; set; }
    }
}
