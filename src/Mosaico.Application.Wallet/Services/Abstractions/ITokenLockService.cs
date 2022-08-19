using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ITokenLockService
    {
        Task<decimal> GetCurrencyLockSumAsync(Guid tokenId, string userId, CancellationToken token = new CancellationToken());
        Task<decimal> GetTokenLockSumAsync(Guid tokenId, string userId, CancellationToken token = new CancellationToken());
        Task SetExpiredAsync(TokenLock tokenLock, CancellationToken token = new CancellationToken());
        Task SetExpiredAsync(Guid lockId, CancellationToken token = new CancellationToken());

        Task<TokenLock> CreateTokenLockAsync(Guid tokenId, string userId, decimal amount, string reason = null,
            DateTimeOffset? expiresAt = null, CancellationToken token = new CancellationToken());

        Task<Dictionary<Guid, decimal>> GetAllTokenLockSumAsync(List<Guid> tokenIds, string userId,
            CancellationToken token = new CancellationToken());

        Task<TokenLock> CreateCurrencyLockAsync(Guid currencyId, string userId, decimal amount, string reason = null,
            DateTimeOffset? expiresAt = null, CancellationToken token = new CancellationToken());

        Task<Dictionary<Guid, decimal>> GetAllCurrencyLockSumAsync(List<Guid> currencyIds, string userId,
            CancellationToken token = new CancellationToken());
    }
}