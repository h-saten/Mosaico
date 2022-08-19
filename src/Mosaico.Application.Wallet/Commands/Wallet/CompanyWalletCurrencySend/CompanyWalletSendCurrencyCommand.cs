using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletCurrencySend
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class CompanyWalletSendCurrencyCommand : IRequest
    {
        public Guid PaymentCurrencyId { get; set; }
        public Guid CompanyId { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
    }
}