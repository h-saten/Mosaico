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
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Checkout.MetamaskCheckoutContext
{
    public class MetamaskCheckoutContextQueryHandler : IRequestHandler<MetamaskCheckoutContextQuery, MetamaskCheckoutContextQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _userContext;
        private readonly IMapper _mapper;

        public MetamaskCheckoutContextQueryHandler(IExchangeRateService exchangeRateService, IProjectWalletService projectWalletService, IProjectManagementClient projectManagementClient, IBusinessManagementClient businessManagement, IWalletDbContext walletDbContext, ICurrentUserContext userContext, IMapper mapper)
        {
            _exchangeRateService = exchangeRateService;
            _projectWalletService = projectWalletService;
            _projectManagementClient = projectManagementClient;
            _businessManagement = businessManagement;
            _walletDbContext = walletDbContext;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<MetamaskCheckoutContextQueryResponse> Handle(MetamaskCheckoutContextQuery request, CancellationToken cancellationToken)
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

            var projectWallet =
                await _walletDbContext.ProjectWallets.FirstOrDefaultAsync(w =>
                    w.ProjectId == project.Id && w.Network == company.Network, cancellationToken: cancellationToken) ??
                await _projectWalletService.CreateWalletAsync(company.Network, project.Id);

            var account = await _projectWalletService.GetAccountAsync(company.Network, project.Id, request.UserId);

            var regulation = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "TERMS_AND_CONDITIONS", _userContext.Language, cancellationToken);
            var policy = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "PRIVACY_POLICY", _userContext.Language, cancellationToken);
            var stage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
            var limit = stage.PurchaseLimits.FirstOrDefault(pl => pl.PaymentMethod == Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet);
            var currencies = await _walletDbContext
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == company.Network)
                .ToListAsync(cancellationToken: cancellationToken);
            
            var projectCurrencies = await _projectManagementClient.GetProjectPaymentCurrenciesAsync(project.Id);
            var nativeCurrencyId = currencies.FirstOrDefault(c => c.NativeChainCurrency && c.Chain == company.Network)?.Id;
            var paymentCurrencies = currencies.Where(p => (projectCurrencies != null && projectCurrencies.Contains(p.ContractAddress)) 
                                                          || p.Id == nativeCurrencyId)
                .Select(c => _mapper.Map<PaymentCurrencyDTO>(c)).ToList();
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
            var exchangeRateDTOs = await GetExchangeRateAsync(token.Symbol, stage.TokenPrice, paymentCurrencies);
            
            return new MetamaskCheckoutContextQueryResponse
            {
                ExchangeRates = exchangeRateDTOs,
                PaymentAddress = account.Address,
                ProjectName = project.Title,
                CompanyName = company.Name,
                RegulationsUrl = regulation?.Url,
                PrivacyPolicyUrl = policy?.Url,
                Currencies = paymentCurrencies,
                MaximumPurchase = limit?.MaximumPurchase ?? stage.MaximumPurchase,
                MinimumPurchase = limit?.MinimumPurchase ?? stage.MinimumPurchase
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