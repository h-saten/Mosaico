using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectTeamMemberDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public int Order { get; set; }
        public string PhotoUrl { get; set; }
        public Guid PageId { get; set; }
    }
}
