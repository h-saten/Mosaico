using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.UpsertExternalExchange
{
    public class UpsertExternalExchangeCommandHandler : IRequestHandler<UpsertExternalExchangeCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;

        public UpsertExternalExchangeCommandHandler(IWalletDbContext walletDbContext, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpsertExternalExchangeCommand request, CancellationToken cancellationToken)
        {
            var externalExchange =
                await _walletDbContext.ExternalExchanges.FirstOrDefaultAsync(t => t.Id == request.ExternalExchangeId,
                    cancellationToken);
            if (externalExchange == null)
            {
                throw new ExternalExchangeNotFoundException(request.ExternalExchangeId.ToString());
            }

            var token = await _walletDbContext.Tokens.Include(t => t.Exchanges)
                .FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);

            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            var tokenToExchange = token.Exchanges.FirstOrDefault(e => e.ExternalExchangeId == externalExchange.Id);
            if (tokenToExchange == null)
            {
                tokenToExchange = new TokenToExternalExchange
                {
                    ExternalExchange = externalExchange,
                    ExternalExchangeId = externalExchange.Id,
                    ListedAt = request.ListedAt,
                    Token = token,
                    TokenId = token.Id
                };
                token.Exchanges.Add(tokenToExchange);
                _walletDbContext.Tokens.Update(token);
            }
            else
            {
                tokenToExchange.ListedAt = request.ListedAt;
            }

            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}