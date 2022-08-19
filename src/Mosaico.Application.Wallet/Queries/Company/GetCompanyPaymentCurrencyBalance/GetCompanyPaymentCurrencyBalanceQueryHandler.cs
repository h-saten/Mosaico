using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyPaymentCurrencyBalance
{
    public class GetCompanyPaymentCurrencyBalanceQueryHandler : IRequestHandler<GetCompanyPaymentCurrencyBalanceQuery, GetCompanyPaymentCurrencyBalanceQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICompanyWalletService _companyWalletService;

        public GetCompanyPaymentCurrencyBalanceQueryHandler(IWalletDbContext walletDbContext, ICompanyWalletService companyWalletService)
        {
            _walletDbContext = walletDbContext;
            _companyWalletService = companyWalletService;
        }

        public async Task<GetCompanyPaymentCurrencyBalanceQueryResponse> Handle(GetCompanyPaymentCurrencyBalanceQuery request, CancellationToken cancellationToken)
        {
            var companyWallet =
                await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
            
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(request.CompanyId.ToString());
            }

            var paymentCurrency =
                await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(t =>
                    t.Chain == companyWallet.Network && t.NativeChainCurrency, cancellationToken: cancellationToken);

            if (paymentCurrency == null)
            {
                throw new UnsupportedPaymentCurrencyException();
            }

            var balance = await _companyWalletService.NativePaymentCurrencyBalanceAsync(companyWallet.AccountAddress,
                paymentCurrency.Ticker, companyWallet.Network);

            return new GetCompanyPaymentCurrencyBalanceQueryResponse
            {
                Balance = balance,
                Network = companyWallet.Network,
                LogoUrl = paymentCurrency.LogoUrl,
                WalletAddress = companyWallet.AccountAddress,
                PaymentCurrencyTicker = paymentCurrency.Ticker
            };
        }
    }
}