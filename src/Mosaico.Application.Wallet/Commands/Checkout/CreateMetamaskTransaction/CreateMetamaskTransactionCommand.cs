using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMetamaskTransaction
{
    public class CreateMetamaskTransactionCommand : IRequest<Guid>
    {
        public string TransactionHash { get; set; }
        public Guid ProjectId { get; set; }
        public string Currency { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal FiatAmount { get; set; }
        public string RefCode { get; set; }
    }
}