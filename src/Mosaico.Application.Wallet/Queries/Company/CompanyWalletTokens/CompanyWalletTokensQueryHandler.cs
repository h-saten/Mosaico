using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using NBitcoin;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTokens
{
    public class CompanyWalletTokensQueryHandler : IRequestHandler<CompanyWalletTokensQuery, CompanyWalletTokensQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICompanyWalletService _companyWalletService;
        
        public CompanyWalletTokensQueryHandler(IWalletDbContext walletDbContext, ICompanyWalletService companyWalletService)
        {
            _walletDbContext = walletDbContext;
            _companyWalletService = companyWalletService;
        }

        public async Task<CompanyWalletTokensQueryResponse> Handle(CompanyWalletTokensQuery request, CancellationToken cancellationToken)
        {
            var companyWallet = await _walletDbContext
                .CompanyWallets
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Stakings)
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Deflation)
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Vestings).ThenInclude(v => v.Funds)
                .FirstOrDefaultAsync(u => u.CompanyId == request.CompanyId, cancellationToken);
            
            if (companyWallet == null)
            {
                companyWallet = await _companyWalletService.CreateCompanyWalletAsync(request.CompanyId,
                    Blockchain.Base.Constants.BlockchainNetworks.Default);
            }
            
            var tokenBalances = await _companyWalletService.GetTokenBalancesAsync(companyWallet, request.TokenTicker, cancellationToken);
            var currentTotalBalance = tokenBalances.Sum(c => c.TotalAssetValue);
            var previousBalance = await _companyWalletService.GetPreviousBalanceAsync(companyWallet.AccountAddress, companyWallet.Network, TimeSpan.FromDays(1));
            
            var deltaDirection = WalletDeltaDirection.NONE;
            var delta = 0m;
            
            if (previousBalance.HasValue)
            {
                if (previousBalance > currentTotalBalance)
                {
                    deltaDirection = WalletDeltaDirection.LOWER;
                    delta = -100 * (currentTotalBalance - previousBalance.Value) / Math.Abs(previousBalance.Value);
                }
                else if (previousBalance < currentTotalBalance && previousBalance > 0)
                {
                    deltaDirection = WalletDeltaDirection.HIGHER;
                    delta = 100 * (currentTotalBalance - previousBalance.Value) / Math.Abs(previousBalance.Value);
                }
            }
            
            return new CompanyWalletTokensQueryResponse
            {
                Delta = Math.Abs(delta),
                DeltaDirection = deltaDirection,
                Tokens = tokenBalances.OrderByDescending(t => t.TotalAssetValue).ToList(),
                Address = companyWallet.AccountAddress,
                Currency = Constants.FIATCurrencies.USD,
                TotalWalletValue = currentTotalBalance
            };
        }
    }
}