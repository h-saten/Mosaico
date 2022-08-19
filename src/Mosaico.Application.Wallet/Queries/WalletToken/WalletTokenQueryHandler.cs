using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.WalletToken
{
    public class WalletTokenQueryHandler : IRequestHandler<WalletTokenQuery, WalletTokenResponse>
    {
        private readonly IWalletDbContext _walletContext;

        public WalletTokenQueryHandler(IWalletDbContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task<WalletTokenResponse> Handle(WalletTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await _walletContext
                .Tokens
                .Where(x => x.Id == request.TokenId)
                .SingleOrDefaultAsync(cancellationToken);

            if (token is null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }
            
            var userWallet = await _walletContext.Wallets
                .Include(w => w.Tokens).ThenInclude(wt => wt.Token)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.Network == token.Network, cancellationToken);

            var response = new WalletTokenResponse
            {
                ConfirmedTransactionAmount = 0
            };
            
            if (userWallet is null)
            {
                return response;
            }
            
            var status = await _walletContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed, cancellationToken);
            var confirmedTransactionsAmount = await _walletContext
                .Transactions
                .Where(x => x.WalletAddress == userWallet.AccountAddress && x.TokenId == request.TokenId && x.Status.Id == status.Id)
                .CountAsync(cancellationToken);
            response.ConfirmedTransactionAmount = confirmedTransactionsAmount;

            return response;
        }
    }
}