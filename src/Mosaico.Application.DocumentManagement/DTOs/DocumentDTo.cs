using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.DTOs
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<DocumentContentDTO> Contents { get; set; } = new List<DocumentContentDTO>();
    }
}
