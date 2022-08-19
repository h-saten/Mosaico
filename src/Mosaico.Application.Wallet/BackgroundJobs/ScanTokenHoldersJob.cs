using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ScanTokenHoldersJob, IsRecurring = true)]
    public class ScanTokenHoldersJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenHoldersIndexer _tokenHoldersIndexer;
        
        public ScanTokenHoldersJob(
            IWalletDbContext walletDbContext,
            ITokenHoldersIndexer tokenHoldersIndexer)
        {
            _walletDbContext = walletDbContext;
            _tokenHoldersIndexer = tokenHoldersIndexer;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var tokensId = await _walletDbContext
                .Tokens
                .AsNoTracking()
                .Where(x => x.Status == TokenStatus.Deployed)
                .Select(x => x.Id)
                .ToListAsync();
            
            foreach (var tokenId in tokensId)
            {
                await _tokenHoldersIndexer.UpdateHoldersAsync(tokenId);
            }
        }
    }
}