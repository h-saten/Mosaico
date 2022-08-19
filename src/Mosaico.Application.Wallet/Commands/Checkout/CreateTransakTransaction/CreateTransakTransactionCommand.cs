using System;
using MediatR;
using Mosaico.Payments.Transak.Models;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateTransakTransaction
{
    public class CreateTransakTransactionCommand : OrderDetails, IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public string RefCode { get; set; }
        public decimal TokenAmount { get; set; }
    }
}