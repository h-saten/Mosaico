using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(LockTokensOnPurchase), "wallets:api")]
    [EventTypeFilter(typeof(SuccessfulPurchaseEvent))]
    public class LockTokensOnPurchase : EventHandlerBase
    {
        private readonly ITokenLockService _lockService;
        private readonly IWalletDbContext _walletDbContext;

        public LockTokensOnPurchase(IWalletDbContext walletDbContext, ITokenLockService lockService)
        {
            _walletDbContext = walletDbContext;
            _lockService = lockService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<SuccessfulPurchaseEvent>();
            if (data != null)
            {
                var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == data.TransactionId);
                if(transaction == null) return;

                if (transaction.TokenId.HasValue && transaction.TokenAmount.HasValue)
                {
                    await _lockService.CreateTokenLockAsync(transaction.TokenId.Value, transaction.UserId,
                        transaction.TokenAmount.Value, Constants.LockReasons.PURCHASE);
                }
            }
        }
    }
}