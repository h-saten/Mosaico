using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.GetTransactionStatus
{
    public class GetTransactionStatusQuery : IRequest<GetTransactionStatusQueryResponse>
    {
        public Guid Id { get; set; }
    }
}