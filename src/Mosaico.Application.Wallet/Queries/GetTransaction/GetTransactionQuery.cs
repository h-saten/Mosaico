using System;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.GetTransaction
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetTransactionQuery : IRequest<TransactionDTO>
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
    }
}