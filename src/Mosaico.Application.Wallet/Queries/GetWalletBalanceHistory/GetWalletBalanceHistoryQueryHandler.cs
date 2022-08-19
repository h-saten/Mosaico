using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Repositories;

namespace Mosaico.Application.Wallet.Queries.GetWalletBalanceHistory
{
    public class GetWalletBalanceHistoryQueryHandler : IRequestHandler<GetWalletBalanceHistoryQuery, GetWalletBalanceHistoryQueryResponse>
    {
        private readonly IWalletBalanceSnapshotRepository _snapshotRepository;
        private readonly IWalletDbContext _walletDbContext;

        public GetWalletBalanceHistoryQueryHandler(IWalletBalanceSnapshotRepository snapshotRepository, IWalletDbContext walletDbContext)
        {
            _snapshotRepository = snapshotRepository;
            _walletDbContext = walletDbContext;
        }

        public async Task<GetWalletBalanceHistoryQueryResponse> Handle(GetWalletBalanceHistoryQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(
                w => w.AccountAddress == request.WalletAddress && w.Network == request.Network, cancellationToken);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(request.WalletAddress, request.Network);
            }

            var to = DateTimeOffset.UtcNow.Date;
            var from = to.AddMonths(-1);

            var snapshots =
                await _snapshotRepository.GetHistoricalSnapshotsAsync(request.Network, request.WalletAddress, from, to);

            return new GetWalletBalanceHistoryQueryResponse
            {
                Balances = snapshots.Select(s => new WalletBalanceHistoryDTO
                {
                    Balance = s.Balance,
                    Date = s.GeneratedAt
                }).ToList()
            };
        }
    }
}