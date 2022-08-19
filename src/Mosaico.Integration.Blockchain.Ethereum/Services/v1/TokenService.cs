using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.ABIs;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.DataTypes;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Extensions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Serilog;

namespace Mosaico.Integration.Blockchain.Ethereum.Services.v1
{
    public class TokenService : ITokenService
    {
        private readonly ILogger _logger;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IEtherScanner _etherScanner;
        private readonly IContractAnalyzer _contractAnalyzer;

        public TokenService(
            IEthereumClientFactory ethereumClientFactory, 
            IEtherScanner etherScanner, 
            IContractAnalyzer contractAnalyzer, 
            ILogger logger = null)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _etherScanner = etherScanner;
            _contractAnalyzer = contractAnalyzer;
            _logger = logger;
        }

        public async Task<TransactionEstimate> EstimateERC20DeploymentAsync(string network, Action<ERC20ContractConfiguration> buildConfig = null)
        {
            var config = GetDefaultERC20Configuration();
            buildConfig?.Invoke(config);
            //TODO: validate config
            _logger?.Verbose($"Trying to resolve ERC20 contract of version {config.Version}");
            var settings = MosaicoERC20v1Extensions.GetSettings(config.Name, config.Symbol, config.InitialSupply,
                config.IsMintable, config.IsBurnable, config.OwnerAddress, isGovernance: config.IsGovernance);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.PrivateKey);
            return await client.GetDeploymentEstimateAsync<MosaicoERC20v1Deployment>(account, settings);
        }

        public async Task<string> DeployERC20Async(string network, Action<ERC20ContractConfiguration> buildConfig = null)
        {
            var config = GetDefaultERC20Configuration();
            buildConfig?.Invoke(config);
            //TODO: validate config
            _logger?.Verbose($"Trying to resolve ERC20 contract of version {config.Version}");
            var settings = MosaicoERC20v1Extensions.GetSettings(config.Name, config.Symbol, config.InitialSupply,
                config.IsMintable, config.IsBurnable, config.OwnerAddress);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.PrivateKey);
            return await client.DeployContractAsync<MosaicoERC20v1Deployment>(account, settings);
        }
        
        public async Task<decimal> GetAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null)
        {
            var config = new AllowanceConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var serviceClient = _ethereumClientFactory.GetServiceFactory(network);
            var ownerAccount = await client.GetAccountAsync(config.OwnerPrivateKey);
            var contract =  await serviceClient.GetServiceAsync<MosaicoERC20v1Service>(config.ContractAddress, ownerAccount);
            var response = await contract.AllowanceQueryAsync(config.OwnerAddress, config.SpenderAddress);
            return Web3.Convert.FromWei(response);
        }

        public async Task SetWalletAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null)
        {
            var config = new AllowanceConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var serviceClient = _ethereumClientFactory.GetServiceFactory(network);
            var ownerAccount = await client.GetAccountAsync(config.OwnerPrivateKey);
            var contract =
                await serviceClient.GetServiceAsync<MosaicoERC20v1Service>(config.ContractAddress, ownerAccount);
            var weiAmount = Web3.Convert.ToWei(config.Amount, config.Decimals);

            _logger?.Information(
                $"[BLOCKCHAIN][ALLOWANCE] msgSender/owner: '{ownerAccount.Address}' | spender: {config.SpenderAddress} | amount: {weiAmount}");

            var response = await contract.ApproveRequestAndWaitForReceiptAsync(config.SpenderAddress, weiAmount);
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error(
                    $"Ethereum Transaction failed for contract {config.ContractAddress} - allowance function");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
        }

        public async Task<decimal> BalanceOfAsync(string network, string contractAddress, string accountAddress)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var admin = await client.GetAdminAccountAsync();
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, admin);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }
            var balanceResponse = await erc20Service.BalanceOfQueryAsync(accountAddress);
            var divisor = (decimal) new BigInteger(Math.Pow(10, decimalsAmount));
            return (decimal) balanceResponse / divisor;
        }
        
        public async Task<TransactionEstimate> EstimateTransferAsync(string network, string contractAddress, string recipientAddress)
        {
            var client = _ethereumClientFactory.GetClient(network);
            return await client.GetTransferEstimateAsync(null, contractAddress, recipientAddress);
        }

        public async Task<string> TransferWithAuthorizationAsync(string network, IAccount sender, string ownerPrivateKey, string contractAddress, string recipient, decimal amount)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var config = _ethereumClientFactory.GetConfiguration(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var name = await erc20Service.NameQueryAsync();
            var decimals = (int)await erc20Service.DecimalsQueryAsync();
            var owner = await client.GetAccountAsync(ownerPrivateKey);
            var currentNonce = await client.GetClient(owner)
                .Eth.Transactions.GetTransactionCount.SendRequestAsync(owner.Address, BlockParameter.CreatePending());
            var nonce = await owner.NonceService.GetNextNonceAsync();
            var transfer = new TransferWithAuthorization
            {
                From = owner.Address,
                To = recipient,
                Value = Web3.Convert.ToWei(amount, decimals),
                ValidAfter = 0,
                ValidBefore = new BigInteger(Math.Floor(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000f) + 3600),
                Nonce = nonce.ToHexByteArray()
            };
            var typedData = USDCDomain.GetTransferWithAuthorizationType(name, "1", contractAddress, BigInteger.Parse(config.Chain));
            
            var key = new EthECKey(ownerPrivateKey);
            var signer = new Eip712TypedDataSigner();
            var signature = signer.SignTypedDataV4(transfer, typedData, key);

            var v = Convert.ToByte(signature[130..132], 16);
            var r = signature[2..66].ConvertHexStringToByteArray();
            var s = signature[66..130].ConvertHexStringToByteArray();

            var web3 = client.GetClient(sender);
            var contract = web3.Eth.GetContract(USDC.ABI, contractAddress);
            var transferWithAuthorizationFunction = contract.GetFunction<TransferWithAuthorizationFunction>();
            var input = new TransferWithAuthorizationFunction
            {
                R = r,
                V = v,
                S = s,
                From = owner.Address,
                To = recipient,
                Nonce = transfer.Nonce,
                Value = transfer.Value,
                ValidAfter = transfer.ValidAfter,
                ValidBefore = transfer.ValidBefore
            };
            var gas = await transferWithAuthorizationFunction.EstimateGasAsync(input, sender.Address, new HexBigInteger(0), new HexBigInteger(0));
            var response = await transferWithAuthorizationFunction
                .SendTransactionAndWaitForReceiptAsync(input, sender.Address, new HexBigInteger(10000000), new HexBigInteger(0), null);
            return response.TransactionHash;
        }
        
        public async Task<string> TransferWithoutReceiptAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var tokensToSend = Web3.Convert.ToWei(amount, decimalsAmount);
            var response = await erc20Service.TransferRequestAsync(recipient, tokensToSend);
            return response;
        }

        public async Task<string> TransferAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var tokensToSend = Web3.Convert.ToWei(amount, decimalsAmount);
            var response = await erc20Service.TransferRequestAndWaitForReceiptAsync(recipient, tokensToSend);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Transaction on {contractAddress} has failed.");
            }

            return response.TransactionHash;
        }

        public async Task<string> ApproveAsync(string network, IAccount sender, string contractAddress, string recipient,
            decimal amount)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var estimate = await _ethereumClientFactory.GetClient(network).GetGasPriceAsync();
            var tokensToSend = Web3.Convert.ToWei(amount, decimalsAmount);
            var response = await erc20Service.ApproveRequestAsync(new ApproveFunction
            {
                Spender = recipient,
                Amount = tokensToSend,
                GasPrice = estimate
            });
            return response;
        }
        
        public async Task<string> BatchTransferAsync(string network, IAccount sender, string contractAddress,
            List<string> recipients, List<decimal> amounts)
        {
            if (recipients.Count != amounts.Count) throw new Exception($"Mismatch in recipients and amounts");
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var tokensToSend = amounts.Select(a => Web3.Convert.ToWei(a, decimalsAmount)).ToList();
            var response = await erc20Service.BatchTransferFromRequestAsync(sender.Address, recipients, tokensToSend);
            return response;
        }

        public async Task<string> BatchTransferAsync(string network, IAccount sender, string contractAddress,
            List<string> recipients, decimal amount)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<MosaicoERC20v1Service>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var tokensToSend = recipients.Select(r => Web3.Convert.ToWei(amount, decimalsAmount)).ToList();
            var response = await erc20Service.BatchTransferFromRequestAndWaitForReceiptAsync(sender.Address, recipients, tokensToSend);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Transaction on {contractAddress} has failed.");
            }

            return response.TransactionHash;
        }

        public async Task<TokenDetails> GetDetailsAsync(string network, string contractAddress)
        {
            var tokenDetails = await _etherScanner.TokenDetailsAsync(network, contractAddress);
            
            string[] functions = {Constants.ERC20ContractFunctions.Burn, Constants.ERC20ContractFunctions.Mint};
            var contractAnalyzeResult =
                await _contractAnalyzer.FunctionsExistsAsync(network, contractAddress, functions);
            tokenDetails.Burnable = contractAnalyzeResult[Constants.ERC20ContractFunctions.Burn];
            tokenDetails.Mintable = contractAnalyzeResult[Constants.ERC20ContractFunctions.Mint];

            return tokenDetails;
        }

        private ERC20ContractConfiguration GetDefaultERC20Configuration()
        {
            return new ERC20ContractConfiguration
            {
                Version = Constants.TokenContractVersions.Version1
            };
        }
    }
}