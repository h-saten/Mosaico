using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Application.Wallet.Queries.WalletTokens
{
    public class WalletTokensQueryHandler : IRequestHandler<WalletTokensQuery, WalletTokensQueryResponse>
    {
        private readonly IUserWalletService _userWalletService;
        private readonly IWalletDbContext _walletContext;
        private readonly ICurrentUserContext _currentUser;

        public WalletTokensQueryHandler(IUserWalletService userWalletService, IWalletDbContext walletContext, ICurrentUserContext currentUser)
        {
            _userWalletService = userWalletService;
            _walletContext = walletContext;
            _currentUser = currentUser;
        }

        public async Task<WalletTokensQueryResponse> Handle(WalletTokensQuery request, CancellationToken cancellationToken)
        {
            var userWallet = await _walletContext.Wallets
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Stakings)
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Deflation)
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token).ThenInclude(t => t.Vestings)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.Network == request.Network, cancellationToken);
            
            userWallet ??= await _userWalletService.CreateWalletAsync(_currentUser.UserId, request.Network);
            
            var tokenBalances = await _userWalletService.GetTokenBalancesAsync(userWallet, request.TokenTicker, cancellationToken);
            //var crowdSaleTokenBalances = await _userWalletService.GetCrowdSaleTokenBalancesAsync(userWallet, cancellationToken);
            
            // foreach (var tokenBalanceDto in tokenBalances)
            // {
            //     if (crowdSaleTokenBalances.ContainsKey(tokenBalanceDto.Id))
            //     {
            //         tokenBalanceDto.Balance += crowdSaleTokenBalances[tokenBalanceDto.Id];
            //     }
            // }
            
            var currentTotalBalance = tokenBalances.Sum(c => c.TotalAssetValue).TruncateDecimals();
            var previousBalance = await _userWalletService.GetPreviousBalanceAsync(userWallet.AccountAddress, userWallet.Network, TimeSpan.FromDays(1));
            
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

            var response = new WalletTokensQueryResponse
            {
                Delta = Math.Abs(delta),
                DeltaDirection = deltaDirection,
                Tokens = tokenBalances.OrderByDescending(t => t.TotalAssetValue).ToList(),
                Address = userWallet.AccountAddress,
                Currency = Constants.FIATCurrencies.USD,
                TotalWalletValue = currentTotalBalance.TruncateDecimals()
            };

            return response;
        }
    }
}