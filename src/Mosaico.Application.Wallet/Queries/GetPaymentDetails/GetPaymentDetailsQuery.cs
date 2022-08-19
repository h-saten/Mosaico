using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.GetPaymentDetails
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetPaymentDetailsQuery : IRequest<GetPaymentDetailsQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}