using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions.Funds;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.VentureFund.GetVentureFundHistory
{
    public class GetVentureFundHistoryQueryHandler : IRequestHandler<GetVentureFundHistoryQuery, GetVentureFundHistoryQueryResponse>
    {        
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVentureFundDbContext _fundDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetVentureFundHistoryQueryHandler(IWalletDbContext walletDbContext, IDateTimeProvider dateTimeProvider, IVentureFundDbContext fundDbContext)
        {
            _walletDbContext = walletDbContext;
            _dateTimeProvider = dateTimeProvider;
            _fundDbContext = fundDbContext;
        }

        public async Task<GetVentureFundHistoryQueryResponse> Handle(GetVentureFundHistoryQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.Now();
            request.To ??= now;
            request.From ??= request.To.Value.AddMonths(-1);
            
            var fund = await _fundDbContext.VentureFunds.Include(vf => vf.Tokens)
                .FirstOrDefaultAsync(t => t.Name == request.Name, cancellationToken);
            
            if (fund == null)
            {
                throw new FundNotFoundException(request.Name);
            }
            
            var tokenHistoryItems = new List<TokenPriceHistoryDTO>();
            foreach (var token in fund.Tokens)
            {
                var rates = await _walletDbContext.ExchangeRates
                    .Where(e => e.CreatedAt >= request.From && e.CreatedAt <= request.To)
                    .Where(e => token.Symbol == e.Ticker && e.BaseCurrency == "USD")
                    .OrderBy(t => t.CreatedAt)
                    .ToListAsync(cancellationToken);
                    
                var tokenHistory = tokenHistoryItems.FirstOrDefault(t => t.TokenSymbol == token.Symbol);
                if (tokenHistory == null)
                {
                    tokenHistory = new TokenPriceHistoryDTO
                    {
                        Currency = "USD",
                        LatestPrice = token.LatestPrice,
                        TokenName = token.Name,
                        TokenSymbol = token.Symbol,
                        Logo = token.Logo,
                        Amount = token.Amount,
                        TotalValue = token.Amount * token.LatestPrice,
                        IsStakingEnabled = token.IsStakingEnabled
                    };
                    var rateGroup = rates.GroupBy(r => r.CreatedAt.Date);
                    foreach (var group in rateGroup)
                    {
                        tokenHistory.Records.Add(new TokenPriceHistoryItem
                        {
                            Date = group.Key,
                            Rate = group.Average(a => a.Rate)
                        });
                    }
                    
                    tokenHistoryItems.Add(tokenHistory);
                }
            }
            
            return new GetVentureFundHistoryQueryResponse
            {
                Tokens = tokenHistoryItems,
                TotalAssetValue = tokenHistoryItems.Sum(h => h.TotalValue),
                LastUpdatedAt = tokenHistoryItems.SelectMany(i => i.Records).Max(t => t.Date)
            };
        }
    }
}