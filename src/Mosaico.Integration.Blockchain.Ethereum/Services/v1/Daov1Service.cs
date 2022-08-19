using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Daov1;
using Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Nethereum.Web3;
using Serilog;
using ERC20Settings = Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition.ERC20Settings;

namespace Mosaico.Integration.Blockchain.Ethereum.Services.v1
{
    public enum ProposalState
    {
        Pending,
        Active,
        Canceled,
        Defeated,
        Succeeded,
        Queued,
        Expired,
        Executed
    };
    
    public class Daov1Service : IDaoService
    {
        private readonly ILogger _logger;
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public Daov1Service(ILogger logger, IEthereumClientFactory ethereumClientFactory)
        {
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
        }
        
        public async Task<TransactionEstimate> EstimateDaoDeploymentAsync(string network, Action<DaoConfiguration> buildConfig = null)
        {
            var config = GetDefaultConfiguration(network);
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            var settings = Daov1Extensions.GetSettings(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.PrivateKey);
            return await client.GetDeploymentEstimateAsync<Daov1Deployment>(account, settings);
        }

        public async Task<string> DeployServiceAsync(string network, Action<DaoConfiguration> buildConfig = null)
        {
            var config = GetDefaultConfiguration(network);
            buildConfig?.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            var settings = Daov1Extensions.GetSettings(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.PrivateKey);
            return await client.DeployContractAsync<Daov1Deployment>(account, settings);
        }

        public async Task AddOwnerAsync(string network, string daoAddress, string ownerAddress, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                privateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(daoAddress, privateKey);
            var response = await daoService.AddOwnerRequestAndWaitForReceiptAsync(ownerAddress);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Adding owner to {daoAddress} has failed");
            }
        }

        public async Task MintTokenAsync(string network, Daov1Configurations.MintTokenConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var amount = Web3.Convert.ToWei(config.Amount, config.Decimals);
            var response = await daoService.MintRequestAndWaitForReceiptAsync(config.ContractAddress, amount);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Mint for the token {config.ContractAddress} failed.");
            }
        }
        
        public async Task BurnTokenAsync(string network, Daov1Configurations.BurnTokenConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var amount = Web3.Convert.ToWei(config.Amount, config.Decimals);
            var response = await daoService.BurnRequestAndWaitForReceiptAsync(config.ContractAddress, amount);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Burn for the token {config.ContractAddress} failed.");
            }
        }

        public async Task<string> CreateProposalAsync(string network, Daov1Configurations.CreateProposalConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var response = await daoService.ProposeRequestAndWaitForReceiptAsync(config.Description, config.ContractAddress);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Proposal for the contract {config.ContractAddress} failed.");
            }

            var proposalId = await daoService.HashProposalQueryAsync(config.Description, config.ContractAddress);
            return proposalId.ToString();
        }

        public async Task<string> VoteAsync(string network, Daov1Configurations.VoteConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var proposalId = BigInteger.Parse(config.ProposalId);
            var response = await daoService.CastVoteRequestAndWaitForReceiptAsync(proposalId, (byte)config.Result);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Vote for proposal {config.ProposalId} failed.");
            }

            return response.TransactionHash;
        }

        public async Task<ProposalState> GetStateAsync(string network, string address, string proposalId, string privateKey = null)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                privateKey = GetAdminPrivateKey(network);
            }
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(address, privateKey);
            var proposalIdBN = BigInteger.Parse(proposalId);
            var response = await daoService.StateQueryAsync(proposalIdBN);
            return (ProposalState) response;
        }

        public async Task AddERC20Async(string network, Daov1Configurations.AddERC20Configuration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var response = await daoService.AddTokenRequestAndWaitForReceiptAsync(config.ContractAddress, config.IsGovernance);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Adding token {config.ContractAddress} to the dao {config.DaoAddress} failed");
            }
        }

        public async Task<List<string>> GetTokensAsync(string network, string daoContract, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                privateKey = GetAdminPrivateKey(network);
            }
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(daoContract, privateKey);
            var result = await daoService.GetTokensQueryAsync();
            return result;
        }

        public async Task<string> CreateERC20Async(string network, Daov1Configurations.CreateERC20Configuration config)
        {
            if (string.IsNullOrWhiteSpace(config.PrivateKey))
            {
                config.PrivateKey = GetAdminPrivateKey(network);
            }
            
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var daoService = await service.GetServiceAsync<Daov1.Daov1Service>(config.DaoAddress, config.PrivateKey);
            var response = await daoService.CreateRequestAsync(new ERC20Settings
            {
                Name = config.Name,
                Symbol = config.Symbol,
                IsBurnable = config.IsBurnable,
                IsGovernance = config.IsGovernance,
                IsMintable = config.IsMintable,
                WalletAddress = config.WalletAddress,
                InitialSupply = Web3.Convert.ToWei(config.InitialSupply, config.Decimals)
            });
            return response;
        }

        private string GetAdminPrivateKey(string network)
        {
            var config = _ethereumClientFactory.GetConfiguration(network);
            return config.AdminAccount.PrivateKey;
        }

        private DaoConfiguration GetDefaultConfiguration(string network)
        {
            var config = _ethereumClientFactory.GetConfiguration(network);
            return new DaoConfiguration
            {
                OnlyOwnerProposals = false,
                Quorum = 20,
                IsVotingEnabled = true,
                InitialVotingDelay = new BigInteger(Constants.BlockTime.GetBlocksForMinutes(config.BlockTime, 1)),
                InitialVotingPeriod = new BigInteger(Constants.BlockTime.GetBlocksForDay(config.BlockTime, 7)),
                PrivateKey = GetAdminPrivateKey(network)
            };
        }
    }
}