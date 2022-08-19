using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(UpdateTokenLogoOnFileUploaded), "wallets:api")]
    [EventTypeFilter(typeof(TokenLogoUploaded))]
    public class UpdateTokenLogoOnFileUploaded : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _context;
        private readonly ICacheClient _cacheClient;

        public UpdateTokenLogoOnFileUploaded(IWalletDbContext walletContext, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = walletContext;
            _cacheClient = cacheClient;
            _logger = logger;
        }
        
        public override async Task HandleAsync(CloudEvent @event)
        {
            var walletEvent = @event?.GetData<TokenLogoUploaded>();
            if (walletEvent != null)
            {
                var token = await _context.Tokens.FirstOrDefaultAsync(p => p.Id == walletEvent.TokenId);
                if (token == null)
                {
                    throw new TokenNotFoundException(walletEvent.TokenId);
                }
                _logger?.Verbose($"Token {walletEvent.TokenId} was found. Attempting to change logo value to {walletEvent.LogoUrl}");
                token.LogoUrl = walletEvent.LogoUrl;
                await _context.SaveChangesAsync();
                _logger?.Verbose($"Token logo was successfully changed");
                await _cacheClient.CleanAsync(new List<string>
                {
                    $"{nameof(GetTokenQuery)}_{token.Id}"
                });
            }
        }
    }
}