using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpsertTokenDistribution
{
    public class UpsertTokenDistributionCommandHandler : IRequestHandler<UpsertTokenDistributionCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenomyService _tokenomyService;
        private readonly ILogger _logger;
        private readonly ITokenDistributionWalletService _distributionWalletService;

        public UpsertTokenDistributionCommandHandler(IWalletDbContext walletDbContext, ITokenomyService tokenomyService, ITokenDistributionWalletService distributionWalletService, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _tokenomyService = tokenomyService;
            _distributionWalletService = distributionWalletService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpsertTokenDistributionCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.Include(t => t.Distributions).FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken: cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            using (var transaction = _walletDbContext.BeginTransaction())
            {
                try
                {
                    foreach (var distributionDto in request.TokenDistributions)
                    {
                        var distribution = token.Distributions.FirstOrDefault(t => t.Id == distributionDto.Id);
                        if (distribution != null)
                        {
                            UpdateDistribution(distribution, token, distributionDto);
                            await _walletDbContext.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            distribution = AddDistribution(token, distributionDto);
                            if (distributionDto.Id.HasValue && distributionDto.Id != Guid.Empty)
                            {
                                distribution.Id = distributionDto.Id.Value;
                                _walletDbContext.Entry(distribution).State = EntityState.Added;
                            }
                            await _walletDbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    token.Distributions.RemoveAll(tokenDistribution => !request.TokenDistributions.Any(td => td.Id == tokenDistribution.Id));
                    await _tokenomyService.ValidateAsync(token, cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private void UpdateDistribution(TokenDistribution distribution, Token token, TokenDistributionDTO dto)
        {
            distribution.Name = dto.Name;
            if (distribution.TokenAmount != dto.TokenAmount && !string.IsNullOrWhiteSpace(distribution.SmartContractId))
            {
                throw new TokenDistributionAlreadyDeployedException(distribution.Id);
            }
            distribution.TokenAmount = dto.TokenAmount;
            _walletDbContext.TokenDistributions.Update(distribution);
        }
        
        private TokenDistribution AddDistribution(Token token, TokenDistributionDTO dto)
        {
            if (token.Distributions.Any(t => string.Equals(t.Name, dto.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new TokenDistributionAlreadyExistsException(dto.Name);
            }
            
            var distribution = new TokenDistribution
            {
                Name = dto.Name,
                Token = token,
                TokenId = token.Id,
                TokenAmount = dto.TokenAmount
            };
            token.Distributions.Add(distribution);
            return distribution;
        }
    }
}