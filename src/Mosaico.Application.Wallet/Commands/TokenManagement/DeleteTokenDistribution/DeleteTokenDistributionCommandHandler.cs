using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeleteTokenDistribution
{
    public class DeleteTokenDistributionCommandHandler : IRequestHandler<DeleteTokenDistributionCommand>
    {
        private readonly IWalletDbContext _walletDbContext;

        public DeleteTokenDistributionCommandHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(DeleteTokenDistributionCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.Include(d => d.Distributions)
                .FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            var distribution = token.Distributions.FirstOrDefault(t => t.Id == request.TokenDistributionId);
            if (distribution == null)
            {
                throw new TokenDistributionNotFoundException(request.TokenDistributionId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(distribution.SmartContractId))
            {
                throw new TokenDistributionAlreadyDeployedException(request.TokenDistributionId);
            }

            _walletDbContext.TokenDistributions.Remove(distribution);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value; 
        }
    }
}