using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class DocumentContentDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}
