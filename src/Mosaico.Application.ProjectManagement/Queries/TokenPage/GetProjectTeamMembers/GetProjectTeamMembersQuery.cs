using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetProjectTeamMembers
{
    // [Cache("{{PageId}}")]
    public class GetProjectTeamMembersQuery : IRequest<GetProjectTeamMembersQueryResponse>
    {
        public Guid PageId { get; set; }
    }
}