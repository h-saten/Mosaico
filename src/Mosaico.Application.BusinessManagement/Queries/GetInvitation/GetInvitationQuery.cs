using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitation
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanRead)]
    public class GetInvitationQuery : IRequest<GetInvitationQueryResponse>
    {
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
    }
}