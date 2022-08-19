using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakingStatistics
{
    public class GetStakingStatisticsQueryHandler : IRequestHandler<GetStakingStatisticsQuery, GetStakingStatisticsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IExchangeRateService _exchangeRateService;

        public GetStakingStatisticsQueryHandler(IWalletDbContext walletDbContext, IExchangeRateService exchangeRateService)
        {
            _walletDbContext = walletDbContext;
            _exchangeRateService = exchangeRateService;
        }

        public async Task<GetStakingStatisticsQueryResponse> Handle(GetStakingStatisticsQuery request, CancellationToken cancellationToken)
        {
            var stakings = await _walletDbContext.Stakings.Where(s => s.UserId == request.UserId &&
                                                                      (s.Status == StakingStatus.Deployed || s.Status == StakingStatus.Withdrawn))
                .ToListAsync(cancellationToken);
            var activeInStaking = 0m;
            var totalInStaking = 0m;
            var rewardClaimed = await _walletDbContext.StakingClaimHistory.Where(t => t.UserId == request.UserId)
                .SumAsync(t => t.Amount, cancellationToken: cancellationToken);

            foreach (var staking in stakings)
            {
                var currency = staking.StakingPair.StakingToken?.Symbol;
                if (staking.StakingPair.Type == StakingPairBaseCurrencyType.Currency)
                {
                    currency = staking.StakingPair.StakingPaymentCurrency?.Ticker;
                }

                if (!string.IsNullOrWhiteSpace(currency))
                {
                    var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(currency);
                    if (exchangeRate != null)
                    {
                        totalInStaking += staking.Balance * exchangeRate.Rate;
                        if (staking.Status == StakingStatus.Deployed)
                        {
                            activeInStaking += staking.Balance * exchangeRate.Rate;
                        }
                    }
                }
            }
            
            return new GetStakingStatisticsQueryResponse
            {
                RewardClaimed = rewardClaimed,
                ActiveStaking = activeInStaking,
                TotalInStaking = totalInStaking
            };
        }
    }
}