using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Application.ProjectManagement.BackgroundJobs.Crowdsale
{
    public class StageDeploymentJob : IStageDeploymentJob
    {
        private readonly IProjectDbContext _context;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IDateTimeProvider _provider;
        private readonly ICrowdsaleService _crowdsaleService;

        public StageDeploymentJob(IProjectDbContext context, ICrowdsaleDispatcher crowdsaleDispatcher, ICrowdsaleService crowdsaleService, IWalletDbContext walletDbContext, IDateTimeProvider provider)
        {
            _context = context;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _crowdsaleService = crowdsaleService;
            _walletDbContext = walletDbContext;
            _provider = provider;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task DeployStageAsync(Guid id, string userId)
        {
            try
            {
                var stage = await _context.Stages
                    .Include(s => s.Project).ThenInclude(p => p.Crowdsale)
                    .Include(s => s.Status)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (stage == null || stage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Closed)
                {
                    throw new StageNotFoundException(id);
                }

                if (stage.DeploymentStatus == StageDeploymentStatus.Deployed && !stage.AllowRedeployment)
                {
                    throw new NotAllowedRedeploymentException(stage.Id.ToString());
                }
                
                var contractAddress = stage.Project.Crowdsale.ContractAddress;
                if (string.IsNullOrWhiteSpace(contractAddress))
                {
                    throw new EmptyContractAddressException(stage.ProjectId.ToString());
                }

                var stageWhiteList = new List<string>();
                if (!stage.Project.TokenId.HasValue)
                {
                    throw new TokenNotFoundException(stage.ProjectId);
                }
                
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == stage.Project.TokenId.Value);
                if (token == null)
                {
                    throw new TokenNotFoundException(stage.Project.TokenId.Value);
                }

                var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                    c.CompanyId == token.CompanyId && c.Network == token.Network);
                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(token.CompanyId, token.Network);
                }

                var nativeExchangeRate = await GetNativeCurrencyRate(stage.TokenPrice, token.Network);
                await _crowdsaleService.StartNewStageAsync(stage.Project.Crowdsale.Network, contractAddress, c =>
                {
                    c.Name = stage.Name;
                    c.Cap = stage.TokensSupply;
                    c.Rate = nativeExchangeRate;
                    c.IsPrivate = stage.Type == StageType.Private;
                    c.MinIndividualCap = stage.MinimumPurchase;
                    c.MaxIndividualCap = stage.MaximumPurchase;
                    c.Whitelist = stageWhiteList;
                    c.StableCoinRate = stage.TokenPrice;
                    c.PrivateKey = companyWallet.PrivateKey;
                });
                stage.DeploymentStatus = StageDeploymentStatus.Deployed;
                stage.DeployedAt = _provider.Now();
                await _context.SaveChangesAsync();
                await _crowdsaleDispatcher.StageDeployed(userId, id);
            }
            catch(Exception ex)
            {
                await _crowdsaleDispatcher.StageDeploymentFailed(userId, id, ex.Message);
            }
        }

        private async Task<decimal> GetNativeCurrencyRate(decimal tokenPrice, string network)
        {
            var paymentCurrency = await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(p => p.NativeChainCurrency && p.Chain == network);
            if (paymentCurrency == null)
            {
                throw new UnsupportedPaymentCurrencyException(network);
            }
            var exchangeRate = await _walletDbContext.ExchangeRates.AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Where(t => t.Ticker == paymentCurrency.Ticker)
                .Select(e => e.Rate)
                .FirstOrDefaultAsync();
            
            if (exchangeRate <= 0)
            {
                throw new InvalidExchangeRateException(network);
            }

            if (tokenPrice <= 0)
            {
                throw new InvalidTokenPriceException("");
            }

            return tokenPrice / exchangeRate;
        }
    }
}