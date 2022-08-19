using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Services
{
    public class TokenomyService : ITokenomyService
    {
        private readonly IProjectManagementClient _managementClient;

        public TokenomyService(IProjectManagementClient managementClient)
        {
            _managementClient = managementClient;
        }

        public async Task<bool> ValidateAsync(Token token, CancellationToken t = new CancellationToken())
        {
            var tokenDistributionSum = token.Distributions.Sum(t => t.TokenAmount);
            if (tokenDistributionSum > token.TotalSupply)
            {
                throw new InvalidTokenomyException("Token distribution is greater than total supply", "TOKEN_DISTR_EXCEEDS_SUPPLY");
            }

            var projects = await _managementClient.GetProjectsByTokenAsync(token.Id, t);
            var stageSupplySum = projects.SelectMany(p => p.Stages).Sum(s => s.TokensSupply);
            if ((stageSupplySum + tokenDistributionSum) > token.TotalSupply)
            {
                throw new InvalidTokenomyException("Project stages exceed total supply", "PROJECT_STAGE_EXCEEDS_SUPPLY");
            }

            return true;
        }
    }
}