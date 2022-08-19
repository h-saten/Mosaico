using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Checkout.KangaCheckoutContext
{
    public class KangaCheckoutContextQueryQueryHandler : IRequestHandler<KangaCheckoutContextQuery, KangaCheckoutContextQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly ICurrentUserContext _userContext;
        private readonly IKangaBuyApiClient _kangaBuyApiClient;
        private readonly IWalletDbContext _dbContext;

        public KangaCheckoutContextQueryQueryHandler(IExchangeRateService exchangeRateService, IProjectManagementClient projectManagementClient, IBusinessManagementClient businessManagement, ICurrentUserContext userContext, IKangaBuyApiClient kangaBuyApiClient, IWalletDbContext dbContext)
        {
            _exchangeRateService = exchangeRateService;
            _projectManagementClient = projectManagementClient;
            _businessManagement = businessManagement;
            _userContext = userContext;
            _kangaBuyApiClient = kangaBuyApiClient;
            _dbContext = dbContext;
        }

        public async Task<KangaCheckoutContextQueryResponse> Handle(KangaCheckoutContextQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var company = await _businessManagement.GetCompanyAsync(project.CompanyId.Value, cancellationToken);
            if (company == null)
            {
                throw new CompanyNotExistsException(project.CompanyId.ToString());
            }
            
            var regulation = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "TERMS_AND_CONDITIONS", _userContext.Language, cancellationToken);
            var policy = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "PRIVACY_POLICY", _userContext.Language, cancellationToken);
            var stage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
            var limit = stage.PurchaseLimits.FirstOrDefault(pl => pl.PaymentMethod == Domain.ProjectManagement.Constants.PaymentMethods.CreditCard);
            
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);

            var tokenSymbol = token.Symbol;
            if (KangaExchange.SDK.Constants.KangaMarketMappingReverse.ContainsKey(tokenSymbol))
            {
                tokenSymbol = KangaExchange.SDK.Constants.KangaMarketMappingReverse[tokenSymbol];
            }
            
            var exchangeRateDTOs = await GetExchangeRateAsync(tokenSymbol);
            
            return new KangaCheckoutContextQueryResponse
            {
                ExchangeRates = exchangeRateDTOs,
                ProjectName = project.Title,
                CompanyName = company.Name,
                RegulationsUrl = regulation?.Url,
                PrivacyPolicyUrl = policy?.Url,
                MaximumPurchase = limit?.MaximumPurchase ?? stage.MaximumPurchase,
                MinimumPurchase = limit?.MinimumPurchase ?? stage.MinimumPurchase,
                Currencies = exchangeRateDTOs.Select(d => new PaymentCurrencyDTO
                {
                    Name = d.Currency,
                    Ticker = d.Currency
                }).ToList()
            };
        }

        private async Task<List<ExchangeRateDTO>> GetExchangeRateAsync(string tokenSymbol)
        {
            var estimate = await _kangaBuyApiClient.GetEstimatesAsync(tokenSymbol);
            var dtos = new List<ExchangeRateDTO>();
            
            if (estimate.Estimates?.oPLN != null && estimate.Estimates.oPLN.TokensAmount > 0)
            {
                dtos.Add(new ExchangeRateDTO
                {
                    Currency = "PLN",
                    BaseCurrency = tokenSymbol,
                    ExchangeRate = Math.Round(estimate.Estimates.oPLN.CurrencyAmount / estimate.Estimates.oPLN.TokensAmount, 6)
                });
            }
            if (estimate.Estimates?.oEUR != null && estimate.Estimates.oEUR.TokensAmount > 0)
            {
                dtos.Add(new ExchangeRateDTO
                {
                    Currency = "EUR",
                    BaseCurrency = tokenSymbol,
                    ExchangeRate = Math.Round(estimate.Estimates.oEUR.CurrencyAmount / estimate.Estimates.oEUR.TokensAmount, 6)
                });
            }
            if (estimate.Estimates?.BTC != null && estimate.Estimates.BTC.TokensAmount > 0)
            {
                dtos.Add(new ExchangeRateDTO
                {
                    Currency = "BTC",
                    BaseCurrency = tokenSymbol,
                    ExchangeRate = Math.Round(estimate.Estimates.BTC.CurrencyAmount / estimate.Estimates.BTC.TokensAmount, 6)
                });
            }
            if (estimate.Estimates?.ETH != null && estimate.Estimates.ETH.TokensAmount > 0)
            {
                dtos.Add(new ExchangeRateDTO
                {
                    Currency = "ETH",
                    BaseCurrency = tokenSymbol,
                    ExchangeRate = Math.Round(estimate.Estimates.ETH.CurrencyAmount / estimate.Estimates.ETH.TokensAmount, 6)
                });
            }

            return dtos;
        }
    }
}