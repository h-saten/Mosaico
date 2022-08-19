using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.Administration.GetAdminProjects
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetAdminProjectsQuery : IRequest<GetAdminProjectsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Status { get; set; }
        public string FreeTextSearch { get; set; }
    }
}