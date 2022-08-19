using System;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Web3;
using Serilog;

namespace Mosaico.Integration.Blockchain.Ethereum.Services.v1
{
    public class CrowdsaleV1Service : ICrowdsaleService
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ILogger _logger;
        
        public CrowdsaleV1Service(IEthereumClientFactory ethereumClientFactory, ILogger logger = null)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _logger = logger;
        }
        
        public async Task<TransactionEstimate> EstimateDeploymentAsync(string network, Action<CrowdsaleContractConfiguration> buildConfig = null)
        {
            var config = await GetDefaultCrowdsaleConfigurationAsync(network);
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            var client = _ethereumClientFactory.GetClient(network);
            var settings = DefaultCrowdsalev1Settings.GetSettings(
                config.OwnerAddress, 
                config.ERC20Address,
                new BigInteger(config.SoftCapDenominator),
                config.SupportedStableCoins,
                new BigInteger(config.StageCount));

            return await client.GetDeploymentEstimateAsync<DefaultCrowdsalev1Deployment>(null, settings);
        }

        public async Task<decimal> WalletBalanceAsync(string network, string contractAddress, string walletAddress)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var contract = await service.GetServiceAsync<DefaultCrowdsalev1Service>(contractAddress, account: null);
            _logger?.Verbose($"Contract {contractAddress} was successfully connected");
            var response = await contract.BalanceOfQueryAsync(walletAddress);
            return response.ConvertToDecimal();
        }

        public async Task<CrowdsaleContractConfiguration> DeployAsync(string network, Action<CrowdsaleContractConfiguration> buildConfig)
        {
            var config = await GetDefaultCrowdsaleConfigurationAsync(network);
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }

            var client = _ethereumClientFactory.GetClient(network);
            var settings = DefaultCrowdsalev1Settings.GetSettings(
                config.OwnerAddress, 
                config.ERC20Address,
                new BigInteger(config.SoftCapDenominator),
                config.SupportedStableCoins,
                new BigInteger(config.StageCount));
            var account = await client.GetAccountAsync(config.PrivateKey);
            var address = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(account, settings);
            config.ContractAddress = address;
            return config;
        }

        public async Task PauseAsync(string network, string contractAddress, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                privateKey = GetAdminPrivateKey(network);
            }
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var contract = await service.GetServiceAsync<DefaultCrowdsalev1Service>(contractAddress, privateKey);
            _logger?.Verbose($"Contract {contractAddress} was successfully connected");
            var response = await contract.PauseRequestAndWaitForReceiptAsync();
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error($"Ethereum Transaction failed for contract {contractAddress} - pause function");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
            _logger?.Verbose($"Smart contract was paused");
        }

        public async Task StartNewStageAsync(string network, string contractAddress, Action<ContractStageConfiguration> buildConfig)
        {
            var config = await GetNewStageDefaultConfigurationAsync();
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);

            var stageFunction = DefaultCrowdsalev1Settings.GetStartNextStageFunction(
                config.Name, 
                config.Cap.ConvertToBigInteger(),
                config.Rate.ConvertToBigInteger(), 
                config.StableCoinRate.ConvertToBigInteger(),
                config.IsPrivate, 
                config.MinIndividualCap.ConvertToBigInteger(), 
                config.MaxIndividualCap.ConvertToBigInteger(),
                config.Whitelist);
            
            var contract = await service.GetServiceAsync<DefaultCrowdsalev1Service>(contractAddress, config.PrivateKey);
            _logger?.Verbose($"Contract {contractAddress} was successfully connected");
            var response = await contract.StartNextStageRequestAndWaitForReceiptAsync(stageFunction);
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error($"Ethereum Transaction failed for contract {contractAddress} - function startNextStage");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
            _logger?.Verbose($"Stage was deployed. Used gas: {response.GasUsed}. Gas price: {response.EffectiveGasPrice}");
        }
        
        public async Task<string> BuyTokens(string network, string contractAddress, Action<BuyTokensConfiguration> buildConfig)
        {
            var config = new BuyTokensConfiguration();
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.SenderPrivateKey))
            {
                config.SenderPrivateKey = GetAdminPrivateKey(network);
            }
            var amount = Web3.Convert.ToWei(config.Amount);
            var buyTokensFunction = DefaultCrowdsalev1Settings.BuyTokensFunction(config.Beneficiary, amount);
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var contract = await service.GetServiceAsync<DefaultCrowdsalev1Service>(contractAddress, config.SenderPrivateKey);
            _logger?.Verbose($"Contract {contractAddress} was successfully connected");

            var response = await contract.BuyTokensRequestAndWaitForReceiptAsync(buyTokensFunction);
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error($"Ethereum Transaction failed for contract {contractAddress} - function buyTokens");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
            // Maybe save it somewhere to has costs audit
            _logger?.Verbose($"Tokens were purchased. Used gas: {response.GasUsed}. Gas price: {response.EffectiveGasPrice}");
            return response.TransactionHash;
        }
        
        public async Task<string> ExchangeTokens(string network, string contractAddress, Action<ExchangeTokensConfiguration> buildConfig)
        {
            var config = new ExchangeTokensConfiguration();
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.SenderPrivateKey))
            {
                config.SenderPrivateKey = GetAdminPrivateKey(network);
            }
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var amount = config.Amount.ConvertToBigInteger(config.PaymentTokenDecimalPlaces);
            var contract = await service.GetServiceAsync<DefaultCrowdsalev1Service>(contractAddress, config.SenderPrivateKey);
            _logger?.Verbose($"Contract {contractAddress} was successfully connected");
            
            var exchangeTokensFunction = new ExchangeTokensFunction
            {
                PaymentToken = config.PaymentTokenAddress,
                Amount = amount,
                Gas = 350000
            };
            
            var contractExist = await _ethereumClientFactory.GetClient(network).ContractExist(contractAddress);
            if (contractExist is false)
            {
                throw new ContractNotFoundException(contractAddress);
            }
            
            _logger?.Information($"[BLOCKCHAIN][EXCHANGE TOKENS] PaymentToken: '{config.PaymentTokenAddress}' | Beneficiary: {config.Beneficiary} | Amount: {amount}");
            var response = await contract.ExchangeTokensRequestAndWaitForReceiptAsync(exchangeTokensFunction);
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error($"Ethereum Transaction failed for contract {contractAddress} - function exchangeTokens");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
            // Maybe save it somewhere to has costs audit
            _logger?.Verbose($"Tokens were exchanged. Used gas: {response.GasUsed}. Gas price: {response.EffectiveGasPrice}");
            return response.TransactionHash;
        }
        
        private async Task<CrowdsaleContractConfiguration> GetDefaultCrowdsaleConfigurationAsync(string network)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var adminAccount = await client.GetAdminAccountAsync();
            var networkConfig = _ethereumClientFactory.GetConfiguration(network);
            return new CrowdsaleContractConfiguration
            {
                OwnerAddress = adminAccount.Address,
                PrivateKey = networkConfig.AdminAccount.PrivateKey,
                IsVestingEnabled = false
            };
        }

        private Task<ContractStageConfiguration> GetNewStageDefaultConfigurationAsync()
        {
            return Task.FromResult(new ContractStageConfiguration());
        }
        
        private string GetAdminPrivateKey(string network)
        {
            var config = _ethereumClientFactory.GetConfiguration(network);
            return config.AdminAccount.PrivateKey;
        }

        // private async Task<CrowdsaleContractConfiguration> DeployCrowdsaleWithVestingAsync(string network, CrowdsaleContractConfiguration config)
        // {
        //     var client = _ethereumClientFactory.GetClient(network);
        //     var settings = DefaultCrowdsalev1Settings.GetSettings(config.OwnerAddress, config.ERC20Address, config.StageCount);
        //     var address = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
        //     config.ContractAddress = address;
        //     return config;
        // }
    }
}