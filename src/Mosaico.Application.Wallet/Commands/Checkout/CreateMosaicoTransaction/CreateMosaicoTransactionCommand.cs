using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMosaicoTransaction
{
    public class CreateMosaicoTransactionCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public string RefCode { get; set; }
        public decimal PayedAmount { get; set; }
        public string Currency { get; set; }
    }
}