using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cronos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;
using Mosaico.Base.Extensions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakings
{
    public class GetStakingsQueryHandler : IRequestHandler<GetStakingsQuery, GetStakingsQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IWalletStakingService _stakingService;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IWalletDbContext _walletDbContext;

        public GetStakingsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper, IDateTimeProvider timeProvider,
            IWalletStakingService stakingService, ICurrentUserContext currentUserContext)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
            _timeProvider = timeProvider;
            _stakingService = stakingService;
            _currentUserContext = currentUserContext;
        }

        public async Task<GetStakingsQueryResponse> Handle(GetStakingsQuery request,
            CancellationToken cancellationToken)
        {
            var now = _timeProvider.Now();
            var stakingPairs = await _walletDbContext.StakingPairs.ToListAsync(cancellationToken);
            var stakings = await _walletDbContext.Stakings
                .Where(t => t.UserId == request.UserId && t.Status == StakingStatus.Deployed)
                .ToListAsync(cancellationToken);
            if (!stakings.Any()) return new GetStakingsQueryResponse();
            var items = new List<StakingDTO>();
            var groups = stakings.GroupBy(s => s.StakingPairId);
            foreach (var group in groups)
            {
                var stakingPair = stakingPairs.FirstOrDefault(p => p.Id == group.Key);
                if (stakingPair != null)
                {
                    var walletTypeGroup = group.GroupBy(t => t.WalletType);
                    foreach (var walletGroup in walletTypeGroup)
                    {
                        if (walletGroup.Key == StakingWallet.METAMASK)
                        {
                            var walletAddresses = walletGroup.GroupBy(t => t.Wallet);
                            foreach (var walletAddress in walletAddresses)
                            {
                                var dto = await ToDTOAsync(cancellationToken, stakingPair, walletAddress.ToList(), now, StakingWallet.METAMASK);
                                items.Add(dto);
                            }
                        }
                        else
                        {
                            var dto = await ToDTOAsync(cancellationToken, stakingPair, walletGroup.ToList(), now, StakingWallet.MOSAICO_WALLET);
                            items.Add(dto);
                        }
                    }
                }
            }

            return new GetStakingsQueryResponse
            {
                Stakings = items
            };
        }

        private async Task<StakingDTO> ToDTOAsync(CancellationToken cancellationToken, StakingPair? stakingPair, IEnumerable<Domain.Wallet.Entities.Staking.Staking> stakings,
            DateTimeOffset now, StakingWallet walletType)
        {
            var dto = new StakingDTO();
            var defaultStake = stakings.FirstOrDefault();
            var stakeUserCount = await _walletDbContext.Stakings
                .Where(t => t.Status == StakingStatus.Deployed && t.StakingPairId == stakingPair.Id)
                .Select(s => s.UserId)
                .Distinct()
                .CountAsync(cancellationToken);
            var stakeTokenCount = await _walletDbContext.Stakings.Where(t =>
                    t.Status == StakingStatus.Deployed && t.StakingPairId == stakingPair.Id)
                .Select(t => t.Balance).SumAsync(cancellationToken);

            dto.Token = _mapper.Map<TokenDTO>(stakingPair.Token);
            dto.Id = stakingPair.Id;
            dto.EstimatedAPR = stakingPair.EstimatedAPR;
            dto.NextRewardAt = GetClosestRewardDate(stakingPair.CronSchedule);
            dto.Version = stakingPair.StakingVersion;
            dto.WalletType = walletType;
            dto.Wallet = defaultStake?.Wallet;
            dto.ContractAddress = stakingPair.ContractAddress;
            dto.TermsAndConditionsUrl = stakingPair.StakingTerms?.GetTranslationInLanguage(_currentUserContext.Language)?.Value;
            dto.StakingRegulation = stakingPair.StakingRegulation?.GetTranslationInLanguage(_currentUserContext.Language)?.Value;
                
            foreach (var stake in stakings)
                dto.Balance += stake.Balance.TruncateDecimals();

            switch (stakingPair.CalculatorType)
            {
                case RewardEstimateCalculatorType.DistributionBased:
                    dto.EstimatedRewardInUSD =
                        stakeUserCount > 0 ? stakingPair.EstimatedRewardInUSD / stakeUserCount : 0;
                    break;
                case RewardEstimateCalculatorType.FixedPercentage:
                    dto.EstimatedRewardInUSD = dto.Balance * (dto.EstimatedAPR / 100);
                    break;
                case RewardEstimateCalculatorType.SmartContract:
                    dto.EstimatedRewardInUSD =
                        await _stakingService.GetEstimatedRewardAsync(defaultStake);
                    break;
                case RewardEstimateCalculatorType.DistributionAndDaysBased:
                    dto.EstimatedRewardInUSD = 0;
                    if (stakeTokenCount > 0)
                    {
                        var paymentDayDifference = DistributionDaysDifference(stakingPair.CronSchedule, dto.NextRewardAt);
                        var dailyPool = stakingPair.EstimatedRewardInUSD / paymentDayDifference;
                        var dailyRewardPerToken = dailyPool / stakeTokenCount;
                        foreach (var stake in stakings)
                        {
                            var daysDifference = (now - stake.CreatedAt).Days;
                            dto.EstimatedRewardInUSD +=
                                daysDifference * dailyRewardPerToken * stake.Balance;
                        }
                    }

                    break;
            }

            return dto;
        }

        private int DistributionDaysDifference(string cron, DateTimeOffset? startTime = null)
        {
            var now = startTime ?? _timeProvider.Now();
            var next = GetClosestRewardDate(cron, now);
            return next.HasValue ? (next.Value - now).Days : 0;
        }

        private DateTimeOffset? GetClosestRewardDate(string cron, DateTimeOffset? startTime = null)
        {
            var now = startTime ?? _timeProvider.Now();
            var expression = CronExpression.Parse(cron);
            return expression.GetNextOccurrence(now, TimeZoneInfo.Utc);
        }
    }
}