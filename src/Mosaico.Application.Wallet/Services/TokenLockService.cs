using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public class TokenLockService : ITokenLockService
    {
        private readonly IWalletDbContext _walletDbContext;

        public TokenLockService(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public Task<decimal> GetCurrencyLockSumAsync(Guid currencyId, string userId, CancellationToken token = new CancellationToken())
        {
            return _walletDbContext.TokenLocks.Where(t => !t.Expired && t.PaymentCurrencyId == currencyId && t.UserId == userId)
                .SumAsync(t => t.Amount, cancellationToken: token);
        }
        
        public Task<decimal> GetTokenLockSumAsync(Guid tokenId, string userId, CancellationToken token = new CancellationToken())
        {
            return _walletDbContext.TokenLocks.Where(t => !t.Expired && t.TokenId == tokenId && t.UserId == userId)
                .SumAsync(t => t.Amount, cancellationToken: token);
        }

        public async Task SetExpiredAsync(TokenLock tokenLock, CancellationToken token = new CancellationToken())
        {
            if (tokenLock != null)
            {
                tokenLock.Expired = true;
                tokenLock.ExpiresAt = DateTimeOffset.UtcNow;
                _walletDbContext.TokenLocks.Update(tokenLock);
                await _walletDbContext.SaveChangesAsync(token);
            }
        }

        public async Task SetExpiredAsync(Guid lockId, CancellationToken token = new CancellationToken())
        {
            var tokenLock = await _walletDbContext.TokenLocks.FirstOrDefaultAsync(t => t.Id == lockId, cancellationToken: token);
            await SetExpiredAsync(tokenLock, token);
        }

        public async Task<TokenLock> CreateTokenLockAsync(Guid tokenId, string userId, decimal amount, string reason = null, DateTimeOffset? expiresAt = null, CancellationToken token = new CancellationToken())
        {
            var tokenLock = new TokenLock
            {
                Amount = amount,
                UserId = userId,
                LockReason = reason,
                TokenId = tokenId,
                ExpiresAt = expiresAt
            };
            _walletDbContext.TokenLocks.Add(tokenLock);
            await _walletDbContext.SaveChangesAsync(token);
            return tokenLock;
        }
        
        public async Task<TokenLock> CreateCurrencyLockAsync(Guid currencyId, string userId, decimal amount, string reason = null, DateTimeOffset? expiresAt = null, CancellationToken token = new CancellationToken())
        {
            var tokenLock = new TokenLock
            {
                Amount = amount,
                UserId = userId,
                LockReason = reason,
                PaymentCurrencyId = currencyId,
                ExpiresAt = expiresAt
            };
            _walletDbContext.TokenLocks.Add(tokenLock);
            await _walletDbContext.SaveChangesAsync(token);
            return tokenLock;
        }

        public Task<Dictionary<Guid, decimal>> GetAllTokenLockSumAsync(List<Guid> tokenIds, string userId, CancellationToken token = new CancellationToken())
        {
            return _walletDbContext.TokenLocks.Where(t => !t.Expired && t.UserId == userId && t.TokenId.HasValue)
                .GroupBy(tl => tl.TokenId.Value)
                .ToDictionaryAsync(t => t.Key, locks => locks.Sum(t => t.Amount), cancellationToken: token);
        }
        
        public Task<Dictionary<Guid, decimal>> GetAllCurrencyLockSumAsync(List<Guid> currencyIds, string userId, CancellationToken token = new CancellationToken())
        {
            return _walletDbContext.TokenLocks.Where(t => !t.Expired && t.UserId == userId && t.PaymentCurrencyId.HasValue)
                .GroupBy(tl => tl.PaymentCurrencyId.Value)
                .ToDictionaryAsync(t => t.Key, locks => locks.Sum(t => t.Amount), cancellationToken: token);
        }
    }
}