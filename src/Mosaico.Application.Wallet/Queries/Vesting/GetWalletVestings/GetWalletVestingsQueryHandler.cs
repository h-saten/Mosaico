using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetWalletVestings
{
    public class GetWalletVestingsQueryHandler : IRequestHandler<GetWalletVestingsQuery, GetWalletVestingsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IDateTimeProvider _timeProvider;

        public GetWalletVestingsQueryHandler(IWalletDbContext walletDbContext, IDateTimeProvider timeProvider)
        {
            _walletDbContext = walletDbContext;
            _timeProvider = timeProvider;
        }

        public async Task<GetWalletVestingsQueryResponse> Handle(GetWalletVestingsQuery request, CancellationToken cancellationToken)
        {
            var now = _timeProvider.Now();
            var userWallets = await _walletDbContext.WalletToVestings
                        .Include(w => w.Vesting).ThenInclude(v => v.Funds)
                        .Include(w => w.Wallet)
                        .Where(t => t.Wallet.Network == request.Network
                            && t.Wallet.UserId == request.UserId)
                        .OrderByDescending(w => w.Vesting.StartsAt).ToListAsync(cancellationToken: cancellationToken);
            if (!userWallets.Any()) return new GetWalletVestingsQueryResponse();
            var items = new List<VestingWalletDTO>();
            foreach (var vesting in userWallets)
            {
                if (vesting.Vesting != null && vesting.Vesting.Funds.Any() && vesting.Vesting.Token != null)
                {
                    var relevantFunds =  vesting.Vesting.Funds
                        .Where(f => f.Status == VestingFundStatus.Deployed || f.Status == VestingFundStatus.Withdrawn)
                        .ToList();
                    if (relevantFunds.Any())
                    {
                        var availableFund = relevantFunds.FirstOrDefault(f => f.Status != VestingFundStatus.Withdrawn && now >= f.StartAt);
                        var nextFund = relevantFunds
                            .Where(t => t.StartAt != null && t.StartAt.Value >= now && t.Status != VestingFundStatus.Withdrawn)
                            .OrderBy(t => t.StartAt.Value).FirstOrDefault();
                        var item = new VestingWalletDTO
                        {
                            Id = vesting.VestingId,
                            TotalPeriod = vesting.Vesting.NumberOfDays,
                            StartsAt = vesting.Vesting.StartsAt,
                            Claimed = relevantFunds.Where(f => f.Status == VestingFundStatus.Withdrawn).Sum(f => f.TokenAmount),
                            Locked = vesting.Vesting.TokenAmount,
                            Token = new TokenDTO
                            {
                                Id = vesting.Vesting.Token.Id,
                                Symbol = vesting.Vesting.Token.Symbol,
                                Name = vesting.Vesting.Token.Name,
                                Address = vesting.Vesting.Token.Address,
                                LogoUrl = vesting.Vesting.Token.LogoUrl
                            },
                            CanClaim = availableFund != null,
                            TokensToClaim = availableFund?.TokenAmount ?? 0,
                            NextUnlock = nextFund?.StartAt
                        };
                        items.Add(item);
                    }
                }
            }
            return new GetWalletVestingsQueryResponse
            {
                Items = items
            };
        }
    }
}