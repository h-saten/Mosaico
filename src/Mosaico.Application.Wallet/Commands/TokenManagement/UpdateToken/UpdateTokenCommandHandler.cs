using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpdateToken
{
    public class UpdateTokenCommandHandler : IRequestHandler<UpdateTokenCommand>
    {
        private readonly IWalletDbContext _walletDbContext;

        public UpdateTokenCommandHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId,
                cancellationToken);
            if (token == null || token.Status == TokenStatus.Deployed)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            token.ContractVersion = request.ContractVersion;
            token.Address = request.ContractAddress;
            token.OwnerAddress = request.OwnerAddress;
            token.Status = TokenStatus.Deployed;
            
            _walletDbContext.Tokens.Update(token);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}