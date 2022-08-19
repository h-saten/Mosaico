using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.GetCompanyPaymentDetails
{
    public class GetCompanyPaymentDetailsQueryHandler : IRequestHandler<GetCompanyPaymentDetailsQuery, GetCompanyPaymentDetailsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;

        public GetCompanyPaymentDetailsQueryHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<GetCompanyPaymentDetailsQueryResponse> Handle(GetCompanyPaymentDetailsQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(
                t => t.CompanyId == request.CompanyId,
                cancellationToken: cancellationToken);
            if (wallet == null)
            {
                throw new CompanyWalletNotFoundException(request.CompanyId.ToString());
            }

            var paymentCurrencyUsdt =
                await _walletDbContext
                    .PaymentCurrencies
                    .FirstOrDefaultAsync(t => t.Ticker == request.Currency && t.Chain == wallet.Network, cancellationToken);
            if (paymentCurrencyUsdt is null)
            {
                throw new UnsupportedCurrencyException(request.Currency);
            }

            return new GetCompanyPaymentDetailsQueryResponse
            {
                AccountAddress = wallet.AccountAddress,
                ContractAddress = paymentCurrencyUsdt.ContractAddress
            };
        }
    }
}