using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetTokenDistribution
{
    public class GetTokenDistributionQueryHandler : IRequestHandler<GetTokenDistributionQuery, List<TokenDistributionDTO>>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVaultv1Service _vaultv1Service;

        public GetTokenDistributionQueryHandler(IWalletDbContext walletDbContext, IVaultv1Service vaultv1Service)
        {
            _walletDbContext = walletDbContext;
            _vaultv1Service = vaultv1Service;
        }

        public async Task<List<TokenDistributionDTO>> Handle(GetTokenDistributionQuery request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.Include(t => t.Distributions).AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.TokenId == token.Id, cancellationToken);
            var basicDistributions = new List<TokenDistributionDTO>();
            foreach (var d in token.Distributions)
            {
                basicDistributions.Add(new TokenDistributionDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    TokenAmount = d.TokenAmount,
                    Blocked = !string.IsNullOrWhiteSpace(d.SmartContractId),
                    Balance = await GetBalanceAsync(d, vault?.Network, vault?.Address)
                });
            }
            return basicDistributions;
        }

        private async Task<decimal?> GetBalanceAsync(TokenDistribution distribution, string network, string vaultAddress)
        {
            if (string.IsNullOrWhiteSpace(distribution.SmartContractId) || string.IsNullOrWhiteSpace(vaultAddress)) return null;
            var balance = await _vaultv1Service.BalanceAsync(network, distribution.SmartContractId, vaultAddress);
            return balance;
        }
    }
}