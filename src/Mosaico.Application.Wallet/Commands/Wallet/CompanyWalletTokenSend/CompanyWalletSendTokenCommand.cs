using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletTokenSend
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class CompanyWalletSendTokenCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public Guid CompanyId { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
    }
}