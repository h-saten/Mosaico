using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableDeflation
{
    public class EnableDeflationCommandHandler : IRequestHandler<EnableDeflationCommand>
    {
        private readonly IWalletDbContext _walletDbContext;

        public EnableDeflationCommandHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(EnableDeflationCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.Include(t => t.Deflation).FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            // if (token.Deflation == null)
            // {
            //     token.Deflation = new Deflation
            //     {
            //         Token = token,
            //         TokenId = token.Id
            //     };
            // }

            token.IsDeflationary = request.IsEnabled;
            // token.Deflation.Type = request.Type;
            // token.Deflation.BuyoutPercentage = request.BuyoutPercentage;
            // token.Deflation.TransactionPercentage = request.TransactionPercentage;
            // token.Deflation.BuyoutDelayInDays = request.BuyoutDelayInDays;
            // token.Deflation.StartsAt = request.StartsAt;
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}