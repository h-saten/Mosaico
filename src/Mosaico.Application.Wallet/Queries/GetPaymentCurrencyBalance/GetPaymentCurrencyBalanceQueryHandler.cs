using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencyBalance
{
    public class GetPaymentCurrencyBalanceQueryHandler : IRequestHandler<GetPaymentCurrencyBalanceQuery, GetPaymentCurrencyBalanceQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserWalletService _userWalletService;
        private readonly ICurrentUserContext _userContext;

        public GetPaymentCurrencyBalanceQueryHandler(IWalletDbContext walletDbContext, IUserWalletService userWalletService, ICurrentUserContext userContext)
        {
            _walletDbContext = walletDbContext;
            _userWalletService = userWalletService;
            _userContext = userContext;
        }

        public async Task<GetPaymentCurrencyBalanceQueryResponse> Handle(GetPaymentCurrencyBalanceQuery request, CancellationToken cancellationToken)
        {
            var userWallet =
                await _walletDbContext.Wallets.FirstOrDefaultAsync(c => c.Network == request.Network && c.UserId == request.UserId, cancellationToken: cancellationToken);
            
            if (userWallet == null)
            {
                throw new WalletNotFoundException(request.UserId);
            }

            var paymentCurrency =
                await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(t =>
                    t.Chain == userWallet.Network && t.NativeChainCurrency, cancellationToken: cancellationToken);

            if (paymentCurrency == null)
            {
                throw new UnsupportedPaymentCurrencyException();
            }

            var balance = await _userWalletService.NativePaymentCurrencyBalanceAsync(userWallet.AccountAddress,
                paymentCurrency.Ticker, userWallet.Network);

            return new GetPaymentCurrencyBalanceQueryResponse
            {
                Balance = balance,
                Network = userWallet.Network,
                LogoUrl = paymentCurrency.LogoUrl,
                WalletAddress = userWallet.AccountAddress,
                PaymentCurrencyTicker = paymentCurrency.Ticker
            };
        }
    }
}