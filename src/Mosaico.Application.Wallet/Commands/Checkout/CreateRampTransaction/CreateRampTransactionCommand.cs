using System;
using MediatR;
using Mosaico.Payments.RampNetwork.Models;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateRampTransaction
{
    public class CreateRampTransactionCommand : RampPurchase, IRequest
    {
        public Guid ProjectId { get; set; }
        public string RefCode { get; set; }
        public decimal TokenAmount { get; set; }
        public string Secret { get; set; }
    }
}