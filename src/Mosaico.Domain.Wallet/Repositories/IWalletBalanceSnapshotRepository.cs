using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.Repositories
{
    public interface IWalletBalanceSnapshotRepository
    {
        Task SaveSnapshotAsync(WalletBalanceSnapshot snapshot);
        Task<List<WalletBalanceSnapshot>> GetHistoricalSnapshotsAsync(string network, string walletAddress, DateTimeOffset? from = null, DateTimeOffset? to = null);
    }
}