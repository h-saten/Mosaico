using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.DeleteExternalExchange
{
    public class DeleteExternalExchangeCommandHandler : IRequestHandler<DeleteExternalExchangeCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;

        public DeleteExternalExchangeCommandHandler(IWalletDbContext walletDbContext, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteExternalExchangeCommand request, CancellationToken cancellationToken)
        {
            var tokenToExternalExchange = await _walletDbContext.TokenToExternalExchanges.FirstOrDefaultAsync(t =>
                t.TokenId == request.TokenId && t.ExternalExchangeId == request.ExternalExchangeId, cancellationToken: cancellationToken);
            if (tokenToExternalExchange == null)
            {
                throw new ExternalExchangeNotFoundException(request.ExternalExchangeId.ToString());
            }

            _walletDbContext.TokenToExternalExchanges.Remove(tokenToExternalExchange);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}