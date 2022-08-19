using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public abstract class GasEstimatorBase : IGasEstimator
    {
        protected readonly IWalletDbContext WalletContext;
        protected readonly IEthereumClientFactory EthereumClientFactory;
        protected readonly ITokenService TokenService;
        protected readonly ICrowdsaleService CrowdsaleService;
        protected readonly IDaoService DaoService;
        protected readonly ILogger Logger;

        protected GasEstimatorBase(IWalletDbContext walletContext, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService, ICrowdsaleService crowdsaleService, ILogger logger, IDaoService daoService)
        {
            WalletContext = walletContext;
            EthereumClientFactory = ethereumClientFactory;
            TokenService = tokenService;
            CrowdsaleService = crowdsaleService;
            Logger = logger;
            DaoService = daoService;
        }
        
        public virtual async Task<List<DeploymentEstimateDTO>> EstimateTransferAsync()
        {
            var estimates = new List<DeploymentEstimateDTO>();
            foreach (var network in EthereumClientFactory.GetAllConfigurations())
            {
                try
                {
                    var client = EthereumClientFactory.GetClient(network.Name);
                    var adminAccount = await client.GetAdminAccountAsync();
                    var nativeCurrency = await WalletContext.PaymentCurrencies.FirstOrDefaultAsync(pc => pc.NativeChainCurrency && pc.Chain == network.Name);
                    if (nativeCurrency == null)
                    {
                        continue;
                    }
            
                    var exchangeRate = await WalletContext.ExchangeRates.FirstOrDefaultAsync(er => er.Ticker == nativeCurrency.Ticker && er.IsCrypto);
                    
                    var configuration = EthereumClientFactory.GetConfiguration(network.Name);
                    if (configuration != null)
                    {
                        var nativeTransferEstimate = await client.GetTransferEstimateAsync(adminAccount, adminAccount.Address);
                        var estimate = await ConvertToEstimateDTOAsync(nativeTransferEstimate, exchangeRate, network.Name,
                            "native_transfer");
                        estimates.Add(estimate);
                    }
                }
                catch (InvalidNetworkException ex)
                {
                    Logger?.Warning($"Failed to estimate for {network.Name}. {ex.Message}");
                }
            }

            return estimates;
        }

        public virtual async Task<List<DeploymentEstimateDTO>> EstimateTransferAsync(string ticker)
        {
            var estimates = new List<DeploymentEstimateDTO>();
            foreach (var network in EthereumClientFactory.GetAllConfigurations())
            {
                try
                {
                    var client = EthereumClientFactory.GetClient(network.Name);
                    var adminAccount = await client.GetAdminAccountAsync();
                    var nativeCurrency = await WalletContext.PaymentCurrencies.FirstOrDefaultAsync(pc => pc.NativeChainCurrency && pc.Chain == network.Name);
                    if (nativeCurrency == null)
                    {
                        continue;
                    }

                    var paymentCurrency =
                        await WalletContext.PaymentCurrencies.FirstOrDefaultAsync(pc =>
                            pc.Ticker == ticker && pc.Chain == network.Name);
                    
                    if (paymentCurrency == null)
                    {
                        continue;
                    }
            
                    var exchangeRate = await WalletContext.ExchangeRates.FirstOrDefaultAsync(er => er.Ticker == nativeCurrency.Ticker && er.IsCrypto);
                    
                    var configuration = EthereumClientFactory.GetConfiguration(network.Name);
                    if (configuration != null)
                    {
                        var erc20TransferEstimate = await TokenService.EstimateTransferAsync(network.Name, paymentCurrency.ContractAddress, adminAccount.Address);
                        var estimate = await ConvertToEstimateDTOAsync(erc20TransferEstimate, exchangeRate, network.Name,
                            Integration.Blockchain.Ethereum.Constants.TokenContractVersions.Version1 + "_transfer");
                        estimates.Add(estimate);
                    }
                }
                catch (InvalidNetworkException ex)
                {
                    Logger?.Warning($"Failed to estimate for {network.Name}. {ex.Message}");
                }
            }

            return estimates;
        }

        protected virtual async Task<DeploymentEstimateDTO> ConvertToEstimateDTOAsync(TransactionEstimate estimate, ExchangeRate exchangeRate, string network, string contractVersion)
        {
            var price = estimate.TransactionFeeInETH * (exchangeRate?.Rate ?? 0);
            
            return new DeploymentEstimateDTO
            {
                Network = network,
                Gas = estimate.Gas,
                GasPrice = estimate.GasPrice,
                ContractVersion = contractVersion,
                Price = price,
                Currency = exchangeRate?.BaseCurrency,
                EstimatedAt = DateTimeOffset.UtcNow,
                PaymentMethod = PaymentMethod,
                Fee = 0,
                NativeCurrencyAmount = estimate.TransactionFeeInETH,
                NativeCurrencyTicker = exchangeRate?.Ticker
            };
        }

        public abstract string PaymentMethod { get; }

        public async Task<List<DeploymentEstimateDTO>> EstimateDeploymentAsync()
        {
            var estimates = new List<DeploymentEstimateDTO>();
            foreach (var network in EthereumClientFactory.GetAllConfigurations())
            {
                try
                {
                    var nativeCurrency = await WalletContext.PaymentCurrencies.FirstOrDefaultAsync(pc => pc.NativeChainCurrency && pc.Chain == network.Name);
                    if (nativeCurrency == null)
                    {
                        continue;
                    }
            
                    var exchangeRate = await WalletContext.ExchangeRates.FirstOrDefaultAsync(er => er.Ticker == nativeCurrency.Ticker && er.IsCrypto);
                    
                    var configuration = EthereumClientFactory.GetConfiguration(network.Name);
                    if (configuration != null)
                    {
                        var config = await EthereumClientFactory.GetAdminAccount(network.Name).GetAdminAccountDetailsAsync();
                        var client = EthereumClientFactory.GetClient(network.Name);
                        var adminAccount = await client.GetAdminAccountAsync();
                        //TODO: to parallel compute
                        
                        var tokenErc20Estimate = await TokenService.EstimateERC20DeploymentAsync(network.Name,
                            c =>
                            {
                                c.Name = "EST";
                                c.Symbol = "EST";
                                c.InitialSupply = BigInteger.One;
                                c.OwnerAddress = adminAccount.Address;
                                c.PrivateKey = config.PrivateKey;
                            });
                        
                        var erc20DeploymentEstimate = await ConvertToEstimateDTOAsync(tokenErc20Estimate, exchangeRate, network.Name,
                            Integration.Blockchain.Ethereum.Constants.TokenContractVersions.Version1);
                        estimates.Add(erc20DeploymentEstimate);

                        var crowdsaleEstimate = await CrowdsaleService.EstimateDeploymentAsync(network.Name, c =>
                        {
                            c.SoftCapDenominator = 30m;
                            c.OwnerAddress = adminAccount.Address;
                            c.StageCount = 1;
                            c.ERC20Address = adminAccount.Address;
                            c.PrivateKey = config.PrivateKey;
                            c.SupportedStableCoins = new List<string> {adminAccount.Address};
                        });
                        
                        var crowdsaleDeploymentEstimate = await ConvertToEstimateDTOAsync(crowdsaleEstimate, exchangeRate, network.Name,
                            Integration.Blockchain.Ethereum.Constants.CrowdsaleContractVersions.Version1);
                        
                        estimates.Add(crowdsaleDeploymentEstimate);

                        var daoEstimate = await DaoService.EstimateDaoDeploymentAsync(network.Name, c =>
                        {
                            c.Name = "EST";
                            c.Owner = adminAccount.Address;
                            c.PrivateKey = config.PrivateKey;
                            c.Quorum = 20;
                            c.InitialVotingDelay = 20;
                            c.InitialVotingPeriod = 20;
                            c.OnlyOwnerProposals = true;
                        });
                        
                        var daoDeploymentEstimate = await ConvertToEstimateDTOAsync(daoEstimate, exchangeRate, network.Name,
                            Integration.Blockchain.Ethereum.Constants.DAOContractVersions.Version1);
                        
                        estimates.Add(daoDeploymentEstimate);
                    }
                }
                catch (InvalidNetworkException ex)
                {
                    Logger?.Warning($"Failed to estimate for {network.Name}. {ex.Message}");
                }
            }

            return estimates;
        }
    }
}