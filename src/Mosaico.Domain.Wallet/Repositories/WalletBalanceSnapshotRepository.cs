using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.Repositories
{
    public class WalletBalanceSnapshotRepository : IWalletBalanceSnapshotRepository
    {
        private readonly ITimeSeriesRepository _seriesRepository;

        public WalletBalanceSnapshotRepository(ITimeSeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        public async Task SaveSnapshotAsync(WalletBalanceSnapshot snapshot)
        {
            var timestamp = snapshot.GeneratedAt.ToUnixTimeSeconds();
            await _seriesRepository.AddAsync($"{snapshot.Network}_{snapshot.WalletAddress}", timestamp, snapshot);
        }

        public async Task<List<WalletBalanceSnapshot>> GetHistoricalSnapshotsAsync(string network, string walletAddress, DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            from ??= DateTimeOffset.MinValue;
            to ??= DateTimeOffset.MaxValue;
            var results = await _seriesRepository.GetAsync<WalletBalanceSnapshot>($"{network}_{walletAddress}", from.Value.ToUnixTimeSeconds(), to.Value.ToUnixTimeSeconds());
            return results;
        }
    }
}