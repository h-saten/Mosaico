using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.TransactionFee
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class TransactionFeeQuery : IRequest<TransactionFeeQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}