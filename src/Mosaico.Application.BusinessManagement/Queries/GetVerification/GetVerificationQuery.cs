using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetVerification
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanRead)]
    public class GetVerificationQuery : IRequest<GetVerificationQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}