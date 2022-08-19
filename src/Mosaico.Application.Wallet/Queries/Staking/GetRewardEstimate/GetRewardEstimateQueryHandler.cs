using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.Staking.GetRewardEstimate
{
    public class GetRewardEstimateQueryHandler : IRequestHandler<GetRewardEstimateQuery, GetRewardEstimateQueryResponse>
    {
        private readonly IWalletStakingService _walletStakingService;
        private readonly IWalletDbContext _walletDbContext;

        public GetRewardEstimateQueryHandler(IWalletStakingService walletStakingService, IWalletDbContext walletDbContext)
        {
            _walletStakingService = walletStakingService;
            _walletDbContext = walletDbContext;
        }

        public async Task<GetRewardEstimateQueryResponse> Handle(GetRewardEstimateQuery request, CancellationToken cancellationToken)
        {
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.Id, cancellationToken);
            
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.Id);
            }

            var stakes = await _walletDbContext.Stakings.Where(s =>
                s.Status == StakingStatus.Deployed && s.UserId == request.UserId && s.StakingPairId == pair.Id).ToListAsync(cancellationToken);
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            var userWallet =
                await _walletDbContext.Wallets.FirstOrDefaultAsync(u =>
                    u.Network == pair.Network && u.UserId == request.UserId, cancellationToken: cancellationToken);
            if (userWallet == null) throw new WalletNotFoundException(request.UserId);

            var balance = await _walletStakingService.GetEstimatedRewardAsync(stakes.FirstOrDefault());
            var paymentCurrency = pair.PaymentCurrencies?.FirstOrDefault();
            return new GetRewardEstimateQueryResponse
            {
                Balance = balance,
                Token = new TokenDTO
                {
                    Address = paymentCurrency?.PaymentCurrency?.ContractAddress,
                    Name = paymentCurrency?.PaymentCurrency?.Name,
                    Network = paymentCurrency?.PaymentCurrency?.Chain,
                    Symbol = paymentCurrency?.PaymentCurrency?.Ticker,
                    Id = paymentCurrency?.PaymentCurrencyId ?? Guid.Empty
                }
            };
        }
    }
}