using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.GetProject
{
    public class GetProjectQueryResponse
    {
        public ProjectDetailDTO Project { get; set; }
        public bool IsSubscribed { get; set; }
    }
}