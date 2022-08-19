using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Base;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserProjects
{
    // [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserProjectsQuery : IRequest<PaginatedResult<ProjectDTO>>
    {
        public string UserId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}