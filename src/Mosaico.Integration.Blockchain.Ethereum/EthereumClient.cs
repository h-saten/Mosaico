using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Extensions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;
using NBitcoin;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Contracts;
using Nethereum.HdWallet;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EthereumClient : EthereumBase, IEthereumClient
    {
        private readonly ILogger _logger;
        public EthereumClient(ILogger logger = null, IValidator<EthereumNetworkConfiguration> configValidator = null) : base(configValidator)
        {
            _logger = logger;
        }

        public async Task<decimal> GetAccountBalanceAsync(string address, CancellationToken token = new CancellationToken())
        {
            var web3 = GetClient();
            var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
            _logger?.Verbose($"Requesting balance of {address}");
            return Web3.Convert.FromWei(balance);
        }

        public async Task<bool> ContractExist(string address, CancellationToken token = new())
        {
            var web3 = GetClient();
            return await web3.ContractExistAsync(address);
        }

        public async Task<EthTransaction> GetTransactionAsync(string hash, CancellationToken token = new CancellationToken())
        {
            var web3 = GetClient();
            var receipt = await web3.Eth.TransactionManager.TransactionReceiptService.PollForReceiptAsync(hash);
            var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(hash);
            return new EthTransaction
            {
                TransactionHash = transaction?.TransactionHash,
                From = transaction?.From,
                To = transaction?.To,
                Value = transaction?.Value?.Value ?? 0,
                Gas = transaction?.Gas?.Value ?? 0,
                BlockHash = transaction?.BlockHash,
                BlockNumber = transaction?.BlockNumber?.Value ?? 0,
                Status = receipt?.Status?.Value == 1? 1 : 0
            };
        }

        public async Task<EthTransaction> GetTransferTransactionAsync(string hash, string accountAddress, string contractAddress, CancellationToken token = new CancellationToken())
        {
            var web3 = GetClient();
            var transaction = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);
            if (transaction == null) return null;
            var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
            var blockParam = new BlockParameter(transaction.BlockNumber);
            var filter = transferEventHandler.CreateFilterInput<string, string>(null, accountAddress, blockParam);
            var events = await transferEventHandler.GetAllChangesAsync(filter);
            var transactionEvent = events.FirstOrDefault(e => e.Log.TransactionHash == hash);
            if (transactionEvent == null) return null;
            return new EthTransaction
            {
                From = transactionEvent.Event.From,
                To = transactionEvent.Event.To,
                TransactionHash = transactionEvent.Log.TransactionHash,
                Value = transactionEvent.Event.Value,
                BlockNumber = transactionEvent.Log.BlockNumber.Value,
                BlockHash = transactionEvent.Log.BlockHash,
                Status = transaction.Status.ToLong(),
                Gas = transaction.GasUsed
            };
        }

        public async Task<List<EthTransaction>> GetContractTransactionsAsync(string accountAddress, string contractAddress, TransactionDirectionType directionType, string latestTransactionHex = null, CancellationToken token = new CancellationToken())
        {
            var block = BlockParameter.CreateEarliest();
            if (!string.IsNullOrWhiteSpace(latestTransactionHex))
            {
                block = new BlockParameter(ulong.Parse(latestTransactionHex) + 1);
            }
            var web3 = GetClient();
            //TODO: how to deal with versioning?
            var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
            var filter = directionType == TransactionDirectionType.Incoming ? 
                transferEventHandler.CreateFilterInput<string, string>(null, accountAddress, block, BlockParameter.CreateLatest())
                : transferEventHandler.CreateFilterInput<string, string>(accountAddress, null, block, BlockParameter.CreateLatest());
            var allTransferEventsForContract = await transferEventHandler.GetAllChangesAsync(filter);
            var transactions = allTransferEventsForContract.Select(log => new EthTransaction
            {
                From = log.Event.From,
                To = log.Event.To,
                TransactionHash = log.Log.TransactionHash,
                Value = log.Event.Value,
                BlockNumber = log.Log.BlockNumber.Value,
                BlockHash = log.Log.BlockHash,
                Type = "Transfer",
                DirectionType = directionType
            }).ToList();
            foreach (var transactionBatch in transactions.GroupBy(t => t.BlockHash))
            {
                var blockHash = transactionBatch.FirstOrDefault()?.BlockHash;
                if (!string.IsNullOrWhiteSpace(blockHash))
                {
                    var transactionBlock = await web3.Eth.Blocks.GetBlockWithTransactionsByHash.SendRequestAsync(blockHash);
                    var timestampt = transactionBlock.Timestamp.ToLong();
                    foreach (var transaction in transactionBatch)
                    {
                        transaction.FinishedAt = DateTimeOffset.FromUnixTimeSeconds(timestampt);
                        transaction.Gas = transactionBlock.Transactions.FirstOrDefault(t => t.TransactionHash == transaction.TransactionHash)?.Gas;
                    }
                }
            }
            return transactions;
        }

        public async Task<TransactionEstimate> GetDeploymentEstimateAsync<TDeployment>(IAccount account = null, Dictionary<string, object> parameters = null, CancellationToken token = new CancellationToken()) where TDeployment : ContractDeploymentMessage, new()
        {
            account ??= await GetAdminAccountAsync(token);
            var client = GetClient(account);
            _logger?.Verbose($"Estimating gas of contract deployment {typeof(TDeployment).Name}");
            var deployment = new TDeployment();
            SetDeploymentParameters(parameters, deployment);
            var deploymentHandler = client.Eth.GetContractDeploymentHandler<TDeployment>();
            var gasEstimate = await deploymentHandler.EstimateGasAsync(deployment);
            var gasPrice = await client.Eth.GasPrice.SendRequestAsync();
            
            var estimate = new TransactionEstimate
            {
                Gas = (decimal) gasEstimate.Value,
                GasPrice = (decimal) gasPrice.Value,
                TransactionFeeInETH = Web3.Convert.FromWei(gasPrice.Value * gasEstimate.Value) 
            };
            return estimate;
        }

        public async Task<string> DeployContractAsync<TDeployment>(IAccount account = null, Dictionary<string, object> parameters = null, CancellationToken token = new CancellationToken()) where TDeployment : ContractDeploymentMessage, new()
        {
            account ??= await GetAdminAccountAsync(token);
            _logger?.Verbose($"Deploying contract {typeof(TDeployment).Name} using {account.Address} address with parameters {JsonConvert.SerializeObject(parameters)}");
            var client = GetClient(account);

            var deployment = new TDeployment();
            SetDeploymentParameters(parameters, deployment);
            var response = await client.Eth.GetContractDeploymentHandler<TDeployment>()
                .SendRequestAndWaitForReceiptAsync(deployment, CancellationTokenSource.CreateLinkedTokenSource(token));
            try
            {
                foreach (var log in response.Logs)
                {
                    _logger?.Verbose(log.ToObject<string>());
                }
            }
            catch (Exception ex)
            {
                _logger?.Warning(ex.Message + "\n" + ex.StackTrace);
            }
            
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Deployment of {typeof(TDeployment).Name}");
            }
            _logger?.Verbose($"Successfully deployed smart contract {typeof(TDeployment).Name} with address {response.ContractAddress}. Transaction hash {response.TransactionHash}");
            _logger?.Information($"For contract deployment of {typeof(TDeployment).Name} spent {response.GasUsed} of gas");
            
            return response.ContractAddress;
        }

        public async Task<string> TransferFundsAsync(string destinationAddress, decimal ethereum, CancellationToken token = new CancellationToken())
        {
            var adminAccount = await GetAdminAccountAsync(token);
            return await TransferFundsAsync(adminAccount, destinationAddress, ethereum, token);
        }

        public Wallet CreateHDWallet(string mnemonic, string password)
        {
            var wallet = new Wallet(mnemonic, password);
            return wallet;
        }

        public Account GetWalletAccount(string seed, string password, int index = 0)
        {
            var wallet = new Wallet(seed, password);
            return wallet.GetAccount(index, GetChain(Configuration.Chain));
        }
        
        public Account CreateWallet()
        {
            var wallet = new Wallet(Wordlist.English, WordCount.Twelve);
            var account = wallet.GetAccount(0, GetChain(Configuration.Chain));
            return account;
        }

        public async Task<TransactionEstimate> GetTransferEstimateAsync(IAccount source, string recipientAddress, CancellationToken token = new CancellationToken())
        {
            source ??= await GetAdminAccountAsync(token);
            var client = GetClient(source);
            var estimate = await client.Eth.Transactions.EstimateGas.SendRequestAsync(new TransactionInput(null, recipientAddress));
            var gasPrice = await client.Eth.GasPrice.SendRequestAsync();
            return new TransactionEstimate
            {
                Gas = (decimal) estimate.Value,
                GasPrice = (decimal) gasPrice.Value,
                TransactionFeeInETH = Web3.Convert.FromWei(gasPrice.Value * estimate) 
            };
        }

        public async Task<TransactionEstimate> GetTransferEstimateAsync(IAccount source, string contractAddress, string recipientAddress, CancellationToken token = new CancellationToken())
        {
            source ??= await GetAdminAccountAsync(token);
            var client = GetClient(source);
            var transferHandler = client.Eth.GetContractTransactionHandler<TransferFunction>();
            var transfer = new TransferFunction
            {
                To = recipientAddress,
                Value = BigInteger.One
            };
            var gasEstimate = await transferHandler.EstimateGasAsync(contractAddress, transfer);
            var gasPrice = await client.Eth.GasPrice.SendRequestAsync();
            
            return new TransactionEstimate
            {
                Gas = (decimal) gasEstimate.Value,
                GasPrice = (decimal) gasPrice.Value,
                TransactionFeeInETH = Web3.Convert.FromWei(gasPrice.Value * gasEstimate) 
            };
        }

        public async Task<string> TransferFundsAsync(IAccount source, string destinationAddress, decimal ethereum, CancellationToken token = new CancellationToken())
        {
            var client = GetClient(source);
            _logger?.Verbose($"Attempting to transfer {ethereum} ETH from {source.Address} to {destinationAddress}");
            var response = await client.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(destinationAddress, ethereum);

            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Transfer to {destinationAddress}");
            }
            _logger?.Verbose($"Successfully transfer {ethereum} ethereum");
            _logger?.Information($"For transfer from {source.Address} to {destinationAddress} spent {response.GasUsed} of gas");
            return response.TransactionHash;
        }
        
        public async Task<string> TransferFundsWithoutReceiptAsync(IAccount source, string destinationAddress, decimal ethereum, CancellationToken token = new CancellationToken())
        {
            var client = GetClient(source);
            _logger?.Verbose($"Attempting to transfer {ethereum} ETH from {source.Address} to {destinationAddress}");
            var response = await client.Eth.GetEtherTransferService().TransferEtherAsync(destinationAddress, ethereum);
            return response;
        }

        public async Task<BigInteger> LatestBlockNumberAsync()
        {
            var client = GetClient();
            var latestBlockNumberHex = await client.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return latestBlockNumberHex.Value;
        }
        
        private static void SetDeploymentParameters<TDeployment>(Dictionary<string, object> parameters, TDeployment deployment) where TDeployment : ContractDeploymentMessage, new()
        {
            if (parameters != null && parameters.Any())
            {
                var deploymentAttributes = deployment.GetType().GetProperties();
                foreach (var propertyInfo in deploymentAttributes)
                {
                    if (propertyInfo.GetCustomAttribute(typeof(ParameterAttribute)) is ParameterAttribute propertyAttribute &&
                        parameters.ContainsKey(propertyAttribute.Name))
                    {
                        propertyInfo.SetValue(deployment, parameters[propertyAttribute.Name]);
                    }
                }
            }
        }
        
        
    }
}