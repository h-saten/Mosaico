using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSaleDetails
{
    public class GetProjectSaleDetailsQueryHandler : IRequestHandler<GetProjectSaleDetailsQuery, GetProjectSaleDetailsResponse>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWalletClient _walletClient;
        private readonly IWalletDbContext _walletDbContext;

        public GetProjectSaleDetailsQueryHandler(IProjectDbContext context, IMapper mapper, IWalletClient walletClient, IWalletDbContext walletDbContext, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _walletClient = walletClient;
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public async Task<GetProjectSaleDetailsResponse> Handle(GetProjectSaleDetailsQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.GetProjectOrThrowAsync(request.UniqueIdentifier, cancellationToken);
            
            if (!project.TokenId.HasValue)
            {
                throw new TokenNotFoundException($"project {project.Id}");
            }
            
            var token = await _walletClient.GetTokenAsync(project.TokenId.Value);
            if (token == null)
            {
                throw new TokenNotFoundException(project.TokenId.Value);
            }

            var currentStage = project.ActiveStage();

            var response = new GetProjectSaleDetailsResponse();
            
            if (currentStage != null)
            {
                _logger.Verbose($"Current stage data taken");
                
                response.Stage = _mapper.Map<StageDTO>(currentStage);
                
                var tokenWalletDetails = await _walletClient.StageTransactionsDetails(token.Id, currentStage.Id);
                response.SoldTokens = tokenWalletDetails.SoldTokensAmount;
                response.PaymentCurrencies = await FetchPaymentCurrenciesAsync(project, currentStage, token.Network);
            }
            
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == project.CompanyId && c.Network == token.Network, cancellationToken: cancellationToken);
            response.CompanyWalletAddress = companyWallet?.AccountAddress;
            response.CompanyWalletNetwork = companyWallet?.Network;

            return response;
        }

        private async Task<List<PaymentCurrencyDTO>> FetchPaymentCurrenciesAsync(Project project, Stage currentStage, string network)
        {
            var crowdSale = project.Crowdsale;
            var currencies = await _walletClient.GetPaymentCurrenciesAsync(network);
            var response = new List<PaymentCurrencyDTO>();
            foreach (var currencyAddress in crowdSale.SupportedStableCoins)
            {
                var paymentCurrencyDetails = currencies.SingleOrDefault(x => String.Equals(x.ContractAddress, currencyAddress, StringComparison.InvariantCultureIgnoreCase));
                if (paymentCurrencyDetails != null)
                {
                    var exchangeRate =
                        await _walletDbContext.ExchangeRates.OrderByDescending(er => er.CreatedAt).Where(t =>
                            t.Ticker == paymentCurrencyDetails.Symbol).Select(t => t.Rate).FirstOrDefaultAsync();
                    if (exchangeRate > 0)
                    {
                        response.Add(new PaymentCurrencyDTO
                        {
                            Name = paymentCurrencyDetails.Name,
                            Symbol = paymentCurrencyDetails.Symbol,
                            ContractAddress = paymentCurrencyDetails.ContractAddress,
                            ExchangeRate = currentStage.TokenPrice / exchangeRate,
                            IsNativeCurrency = paymentCurrencyDetails.IsNativeCurrency
                        });
                    }
                }
            }

            var nativeCurrency = currencies.SingleOrDefault(x => x.IsNativeCurrency);

            if (nativeCurrency is not null)
            {
                var exchangeRate =
                    await _walletDbContext.ExchangeRates.OrderByDescending(er => er.CreatedAt).Where(t =>
                        t.Ticker == nativeCurrency.Symbol && t.IsCrypto).Select(t => t.Rate).FirstOrDefaultAsync();
                if (exchangeRate > 0)
                {
                    response.Add(new PaymentCurrencyDTO
                    {
                        Name = nativeCurrency.Name,
                        Symbol = nativeCurrency.Symbol,
                        ContractAddress = nativeCurrency.ContractAddress,
                        ExchangeRate = currentStage.TokenPrice / exchangeRate,
                        IsNativeCurrency = nativeCurrency.IsNativeCurrency
                    });
                }
            }

            return response;
        }
    }
}