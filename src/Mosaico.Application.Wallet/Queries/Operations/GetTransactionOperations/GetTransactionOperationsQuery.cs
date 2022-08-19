using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.Operations.GetTransactionOperations
{
    public class GetTransactionOperationsQuery : IRequest<GetTransactionOperationsQueryResponse>
    {
        public Guid TransactionId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}