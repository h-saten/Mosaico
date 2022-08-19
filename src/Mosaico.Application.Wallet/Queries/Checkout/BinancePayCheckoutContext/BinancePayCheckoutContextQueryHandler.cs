using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Checkout.BinancePayCheckoutContext
{
    public class BinancePayCheckoutContextQueryHandler : IRequestHandler<BinancePayCheckoutContextQuery, BinancePayCheckoutContextQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly ICurrentUserContext _userContext;
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;

        public BinancePayCheckoutContextQueryHandler(ICurrentUserContext userContext, IBusinessManagementClient businessManagement, IProjectManagementClient projectManagementClient, IExchangeRateService exchangeRateService, IWalletDbContext context, IMapper mapper)
        {
            _userContext = userContext;
            _businessManagement = businessManagement;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BinancePayCheckoutContextQueryResponse> Handle(BinancePayCheckoutContextQuery request, CancellationToken cancellationToken)
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
            var limit = stage.PurchaseLimits.FirstOrDefault(pl => pl.PaymentMethod == Domain.ProjectManagement.Constants.PaymentMethods.Binance);
            
            var paymentCurrencies = new List<PaymentCurrencyDTO>
            {
                new PaymentCurrencyDTO
                {
                    Ticker = "USDT",
                    Name = "USDT",
                    Network = "BSC",
                    Decimals = 18
                },
                new PaymentCurrencyDTO
                {
                    Ticker = "BUSD",
                    Name = "BUSD",
                    Network = "BSC",
                    Decimals = 18
                }
            };
            
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
            var exchangeRateDTOs = await GetExchangeRateAsync(token.Symbol, stage.TokenPrice, paymentCurrencies);
            
            return new BinancePayCheckoutContextQueryResponse
            {
                ExchangeRates = exchangeRateDTOs,
                ProjectName = project.Title,
                CompanyName = company.Name,
                RegulationsUrl = regulation?.Url,
                PrivacyPolicyUrl = policy?.Url,
                MaximumPurchase = limit?.MaximumPurchase ?? stage.MaximumPurchase,
                MinimumPurchase = limit?.MinimumPurchase ?? stage.MinimumPurchase,
                Currencies = paymentCurrencies
            };
        }
        
        private async Task<List<ExchangeRateDTO>> GetExchangeRateAsync(string tokenSymbol, decimal tokenPriceInUsd, List<PaymentCurrencyDTO> paymentCurrencyDTOs)
        {
            if (tokenPriceInUsd <= 0) return new List<ExchangeRateDTO>();
            if (paymentCurrencyDTOs == null || !paymentCurrencyDTOs.Any()) return new List<ExchangeRateDTO>();
            var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(whitelisted: paymentCurrencyDTOs.Select(c => c.Ticker).ToList());
            var dtos = new List<ExchangeRateDTO>();
            foreach (var exchangeRate in exchangeRates)
            {
                dtos.Add(new ExchangeRateDTO
                {
                    Currency = exchangeRate.Ticker,
                    BaseCurrency = tokenSymbol,
                    ExchangeRate = Math.Round(tokenPriceInUsd / exchangeRate.Rate, 6)
                });
            }

            return dtos;
        }
    }
}