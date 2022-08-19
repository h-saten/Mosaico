using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectMembers
{
    public class GetProjectMembersQueryResponse
    {
        public List<ProjectMemberDTO> Entities { get; set; }
    }
}