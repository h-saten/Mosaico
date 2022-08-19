using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(UpdateTermsUrlOnUploaded),  "wallets:api")]
    [EventTypeFilter(typeof(StakingTermsUploaded))]
    public class UpdateTermsUrlOnUploaded : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;

        public UpdateTermsUrlOnUploaded(IWalletDbContext walletDbContext, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<StakingTermsUploaded>();
            if (data != null)
            {
                _logger?.Information($"Trying to update staking terms for {data.StakingPairId} and language {data.Language} with document: {data.DocumentUrl}");
                var staking = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == data.StakingPairId);
                if (staking != null)
                {
                    _logger?.Information($"Pair was found.");
                    staking.StakingTerms ??= new StakingTerms();
                    staking.StakingTerms.UpdateTranslation(data.DocumentUrl, data.Language);
                    _walletDbContext.StakingPairs.Update(staking);
                    await _walletDbContext.SaveChangesAsync();
                    _logger?.Information($"Staking terms were updated");
                }
            }
        }
    }
}