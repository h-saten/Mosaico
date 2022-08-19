using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(VerifySoftCapOnPurchase), "wallets:api")]
    [EventTypeFilter(typeof(SuccessfulPurchaseEvent))]
    public class VerifySoftCapOnPurchase : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectManagementClient _projectManagementClient;

        public VerifySoftCapOnPurchase(IProjectManagementClient projectManagementClient, IWalletDbContext walletDbContext)
        {
            _projectManagementClient = projectManagementClient;
            _walletDbContext = walletDbContext;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<SuccessfulPurchaseEvent>();
            if (data != null)
            {
                var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == data.TransactionId);
                if (transaction != null && transaction.TokenId.HasValue && transaction.ProjectId.HasValue)
                {
                    var project = await _projectManagementClient.GetProjectDetailsAsync(transaction.ProjectId.Value);
                    if (project != null && project.HardCap > 0 && project.SoftCap > 0)
                    {
                        var soldTokensQuery = _walletDbContext
                            .Transactions
                            .AsNoTracking()
                            .Include(m => m.Status)
                            .Include(m => m.Type)
                            .Where(t => 
                                t.TokenId == transaction.TokenId && transaction.ProjectId == t.ProjectId &&
                                t.PayedAmount != null
                                && t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase
                                && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
                        var totalTokenAmount = await soldTokensQuery.SumAsync(m => m.TokenAmount);
                        var raisedCapitalSoftCapPercentage = project.SoftCap > 0 ? totalTokenAmount * 100 / project.SoftCap : 0;
                        var isSoftCapAchieved = raisedCapitalSoftCapPercentage >= 100;
                        if (isSoftCapAchieved)
                        {
                            await ExpireLockedPurchaseTokensAsync(transaction.TokenId.Value);
                        }
                    }
                }
            }
        }

        private async Task ExpireLockedPurchaseTokensAsync(Guid tokenId)
        {
            var tokenLocks = await _walletDbContext.TokenLocks.Where(t =>
                !t.Expired && t.TokenId == tokenId &&
                t.LockReason == Constants.LockReasons.PURCHASE).ToListAsync();
            var now = DateTimeOffset.UtcNow;
            foreach (var tokenLock in tokenLocks)
            {
                tokenLock.Expired = true;
                tokenLock.ExpiresAt = now;
            }
            await _walletDbContext.SaveChangesAsync();
        }
    }
}