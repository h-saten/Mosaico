using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectMembers
{
    // [Cache("{{ProjectId}}")]
    public class GetProjectMembersQuery : IRequest<GetProjectMembersQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}