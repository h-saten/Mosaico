using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitations
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanRead)]
    public class GetInvitationsQuery : IRequest<GetCompanyInvitationsQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}