using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableStaking
{
    public class EnableStakingCommandHandler : IRequestHandler<EnableStakingCommand>
    {
        private readonly IWalletDbContext _walletDbContext;

        public EnableStakingCommandHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(EnableStakingCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            token.IsStakingEnabled = request.IsEnabled;
            token.StakingStartsAt = request.StartsAt;
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}