using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetHistoricalExchangeRate
{
    public class GetHistoricalExchangeRateQueryHandler : IRequestHandler<GetHistoricalExchangeRateQuery, GetHistoricalExchangeRateQueryResponse>
    {
        private readonly IWalletDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetHistoricalExchangeRateQueryHandler(IWalletDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<GetHistoricalExchangeRateQueryResponse> Handle(GetHistoricalExchangeRateQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.Now();
            request.To ??= now;
            request.From ??= request.To.Value.AddMonths(-1);

            var rates = await _context.ExchangeRates
                .Where(e => e.CreatedAt >= request.From && e.CreatedAt <= request.To)
                .Where(e => request.Symbol == e.Ticker && e.BaseCurrency == "USD")
                .OrderBy(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
            var records = new List<TokenPriceHistoryItem>();
            var rateGroup = rates.GroupBy(r => r.CreatedAt.Date);
            foreach (var group in rateGroup)
            {
                records.Add(new TokenPriceHistoryItem
                {
                    Date = group.Key,
                    Rate = group.Average(a => a.Rate)
                });
            }
            var latestPrice = await _context.ExchangeRates.OrderByDescending(t => t.CreatedAt)
                .Where(t => t.Ticker == request.Symbol && t.BaseCurrency == "USD").Select(t => t.Rate)
                .FirstOrDefaultAsync(cancellationToken);
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Symbol == request.Symbol, cancellationToken);
            if (token != null)
            {
                return new GetHistoricalExchangeRateQueryResponse
                {
                    Records = records,
                    Currency = "USD",
                    Logo = token.LogoUrl,
                    LatestPrice = latestPrice,
                    TokenId = token.Id,
                    TokenName = token.Name,
                    TokenSymbol = token.Symbol
                };
            }

            var paymentCurrency =
                await _context.PaymentCurrencies.FirstOrDefaultAsync(t => t.Ticker == request.Symbol,
                    cancellationToken);
            if (paymentCurrency != null)
            {
                return new GetHistoricalExchangeRateQueryResponse
                {
                    Records = records,
                    Currency = "USD",
                    Logo = paymentCurrency.LogoUrl,
                    LatestPrice = latestPrice,
                    TokenId = paymentCurrency.Id,
                    TokenName = paymentCurrency.Name,
                    TokenSymbol = paymentCurrency.Ticker
                };
            }

            return new GetHistoricalExchangeRateQueryResponse
            {
                Records = records
            };
        }
    }
}