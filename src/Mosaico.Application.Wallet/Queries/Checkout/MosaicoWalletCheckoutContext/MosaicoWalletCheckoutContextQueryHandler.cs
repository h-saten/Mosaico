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
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Checkout.MosaicoWalletCheckoutContext
{
    public class MosaicoWalletCheckoutContextQueryHandler : IRequestHandler<MosaicoWalletCheckoutContextQuery, MosaicoWalletCheckoutContextQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly ICurrentUserContext _userContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserWalletService _userWalletService;
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;
        public MosaicoWalletCheckoutContextQueryHandler(IExchangeRateService exchangeRateService, IProjectManagementClient projectManagementClient, IBusinessManagementClient businessManagement, ICurrentUserContext userContext, IWalletDbContext context, IUserWalletService userWalletService, IAccountRepository accountRepository, IMapper mapper)
        {
            _exchangeRateService = exchangeRateService;
            _projectManagementClient = projectManagementClient;
            _businessManagement = businessManagement;
            _userContext = userContext;
            _context = context;
            _userWalletService = userWalletService;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<MosaicoWalletCheckoutContextQueryResponse> Handle(MosaicoWalletCheckoutContextQuery request, CancellationToken cancellationToken)
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
            var limit = stage.PurchaseLimits.FirstOrDefault(pl => pl.PaymentMethod == Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet);
            var currencies = await _context
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == company.Network)
                .ToListAsync(cancellationToken: cancellationToken);
            
            var projectCurrencies = await _projectManagementClient.GetProjectPaymentCurrenciesAsync(project.Id);
            var nativeCurrencyId = currencies.FirstOrDefault(c => c.NativeChainCurrency && c.Chain == company.Network)?.Id;
            var paymentCurrencies = currencies.Where(p => (projectCurrencies != null && projectCurrencies.Contains(p.ContractAddress)) 
                                                          || p.Id == nativeCurrencyId)
                .Select(c => _mapper.Map<PaymentCurrencyDTO>(c)).ToList();
            
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
            var exchangeRateDTOs = await GetExchangeRateAsync(token.Symbol, stage.TokenPrice, paymentCurrencies);

            var userWallet = await _userWalletService.GetWalletAsync(_userContext.UserId, company.Network);
            var balanceTasks = paymentCurrencies.Select(paymentCurrency => GetCurrencyBalanceAsync(paymentCurrency, userWallet)).ToList();
            await Task.WhenAll(balanceTasks);
            var balances = balanceTasks.Select(bt => bt.Result).ToList();

            return new MosaicoWalletCheckoutContextQueryResponse
            {
                ExchangeRates = exchangeRateDTOs,
                ProjectName = project.Title,
                CompanyName = company.Name,
                RegulationsUrl = regulation?.Url,
                PrivacyPolicyUrl = policy?.Url,
                MaximumPurchase = limit?.MaximumPurchase ?? stage.MaximumPurchase,
                MinimumPurchase = limit?.MinimumPurchase ?? stage.MinimumPurchase,
                Currencies = paymentCurrencies,
                PaymentCurrencyBalances = balances
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

        private async Task<TokenBalanceDTO> GetCurrencyBalanceAsync(PaymentCurrencyDTO paymentCurrency, Domain.Wallet.Entities.Wallet userWallet)
        {
            var balance = 0m;
            if (paymentCurrency.NativeChainCurrency)
            {
                balance = (await _accountRepository.AccountBalanceAsync(userWallet.AccountAddress,
                    paymentCurrency.Network)).Balance;
            }
            else
            {
                var hexBalance = await _accountRepository.Erc20BalanceAsync(userWallet.AccountAddress,
                    paymentCurrency.ContractAddress, userWallet.Network);
                balance = new Wei(hexBalance, paymentCurrency.Decimals).ToDecimal();
            }
            
            return new TokenBalanceDTO
            {
                Balance = balance,
                Currency = Constants.FIATCurrencies.USD,
                Name = paymentCurrency.Name,
                Network = paymentCurrency.Network,
                Symbol = paymentCurrency.Ticker,
                ContractAddress = paymentCurrency.ContractAddress,
                IsPaymentCurrency = true
            };
        }
    }
}