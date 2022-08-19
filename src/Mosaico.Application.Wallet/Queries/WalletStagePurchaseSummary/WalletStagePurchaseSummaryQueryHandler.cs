using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.WalletStagePurchaseSummary
{
    public class WalletStagePurchaseSummaryQueryHandler : IRequestHandler<WalletStagePurchaseSummaryQuery, WalletStagePurchaseSummaryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public WalletStagePurchaseSummaryQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<WalletStagePurchaseSummaryResponse> Handle(WalletStagePurchaseSummaryQuery request, CancellationToken cancellationToken)
        {
            var userWallet = await _walletDbContext
                .Wallets
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.Network == request.Network, cancellationToken);
            
            if (userWallet == null)
            {
                throw new WalletNotFoundException(request.UserId, request.Network);
            }

            var stageId = Guid.Parse(request.StageId);
            var tokensPurchasedSum = await _walletDbContext
                .Transactions
                .Include(t => t.Status)
                .Include(t => t.Type)
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .Where(m =>
                    m.WalletAddress == userWallet.AccountAddress
                    && m.Network == request.Network
                    && m.StageId == stageId)
                .SumAsync(t => t.TokenAmount, cancellationToken);
            
            return new WalletStagePurchaseSummaryResponse
            {
                TokensPurchased = tokensPurchasedSum ?? Decimal.Zero
            };
        }
    }
}