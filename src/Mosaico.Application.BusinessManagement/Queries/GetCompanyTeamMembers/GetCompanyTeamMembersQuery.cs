using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyTeamMembers
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanRead)]
    public class GetCompanyTeamMembersQuery : IRequest<GetCompanyTeamMembersQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}