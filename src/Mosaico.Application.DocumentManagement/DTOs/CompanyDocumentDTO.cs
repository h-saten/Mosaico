using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.DTOs
{
    public class CompanyDocumentDTO : DocumentDTO
    {
        public Guid CompanyId { get; set; }
        public bool IsMandatory { get; set; }
    }
}
