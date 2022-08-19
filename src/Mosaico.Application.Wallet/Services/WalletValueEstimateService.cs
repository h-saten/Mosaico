using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Services
{
    public class WalletValueEstimateService : IWalletValueEstimateService
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserWalletService _userWalletService;

        public WalletValueEstimateService(IWalletDbContext walletDbContext, IUserWalletService userWalletService)
        {
            _walletDbContext = walletDbContext;
            _userWalletService = userWalletService;
        }

        public async Task<decimal> GetWalletValueAsync(Guid companyWalletId, string paymentCurrency = Domain.Wallet.Constants.PaymentCurrencies.USDT, CancellationToken cancellationToken = new CancellationToken())
        {
            var wallet = await _walletDbContext.CompanyWallets
                .Include(w => w.Tokens).ThenInclude(t => t.Token)
                .FirstOrDefaultAsync(c => c.Id == companyWalletId, cancellationToken);
            
            if (wallet == null)
            {
                throw new CompanyWalletNotFoundException(companyWalletId.ToString());
            }

            var estimatedValue = new decimal(0);
            var usdtBalance = await _userWalletService.PaymentCurrencyBalanceAsync(wallet.AccountAddress, paymentCurrency, wallet.Network);
            estimatedValue += usdtBalance;
            

            return estimatedValue;
        }
    }
}