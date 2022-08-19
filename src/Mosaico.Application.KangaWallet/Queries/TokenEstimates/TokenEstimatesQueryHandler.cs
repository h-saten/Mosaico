using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Application.KangaWallet.Queries.TokenEstimates
{
    public class TokenEstimatesQueryHandler : IRequestHandler<TokenEstimatesQuery, TokenEstimatesResponseDto>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IKangaBuyApiClient _kangaBuyApiClient;
        private readonly ILogger _logger;

        public TokenEstimatesQueryHandler(IWalletDbContext walletDbContext, IKangaBuyApiClient kangaBuyApiClient, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _kangaBuyApiClient = kangaBuyApiClient;
            _logger = logger;
        }

        public async Task<TokenEstimatesResponseDto> Handle
            (TokenEstimatesQuery request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                _logger?.Fatal($"Potential breach. User tries to buy tokens when he is unauthorized");
                throw new TokenNotFoundException(request.TokenId.ToString());
            }

            var tokenTicker = token.Symbol;
            
            var estimates = await _kangaBuyApiClient.GetEstimatesAsync(tokenTicker);

            List<CurrencyEstimateDto> processedEstimates = ProcessEstimatesToPriceBasedModel(estimates);

            var response = new TokenEstimatesResponseDto
            {
                Estimates = processedEstimates
            };
            
            return response;
        }

        private List<CurrencyEstimateDto> ProcessEstimatesToPriceBasedModel(EstimatesResponseDto estimates)
        {
            var response = new List<CurrencyEstimateDto>();
            var estimatesValues = estimates.Estimates;
            if (estimatesValues.BTC != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.BTC,
                    Price = estimatesValues.BTC.CurrencyAmount / estimatesValues.BTC.TokensAmount,
                });
            }
            if (estimatesValues.ETH != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.ETH,
                    Price = estimatesValues.ETH.CurrencyAmount / estimatesValues.ETH.TokensAmount,
                });
            }
            if (estimatesValues.oEUR != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.EUR,
                    Price = estimatesValues.oEUR.CurrencyAmount / estimatesValues.oEUR.TokensAmount,
                });
            }
            if (estimatesValues.oPLN != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.PLN,
                    Price = estimatesValues.oPLN.CurrencyAmount / estimatesValues.oPLN.TokensAmount,
                });
            }
            if (estimatesValues.oUSD != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.USD,
                    Price = estimatesValues.oUSD.CurrencyAmount / estimatesValues.oUSD.TokensAmount,
                });
            }
            if (estimatesValues.USDT != null)
            {
                response.Add(new CurrencyEstimateDto
                {
                    CurrencyTicker = KangaPaymentCurrency.USDT,
                    Price = estimatesValues.USDT.CurrencyAmount / estimatesValues.USDT.TokensAmount,
                });
            }

            return response;
        }
    }
}
