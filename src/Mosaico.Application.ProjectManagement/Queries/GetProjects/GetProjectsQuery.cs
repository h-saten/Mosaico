using MediatR;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjects
{
    public class GetProjectsQuery : IRequest<GetProjectsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Status { get; set; }
        public string TextSearch { get; set; }
        public bool LandingOnly { get; set; }
    }
}