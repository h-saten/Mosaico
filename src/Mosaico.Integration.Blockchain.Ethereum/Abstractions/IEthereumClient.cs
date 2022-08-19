using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Nethereum.Contracts;
using Nethereum.HdWallet;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IEthereumClient
    {
        IAdminAccountProvider<EthereumAdminAccount> AdminAccountProvider { get; set; }
        EthereumNetworkConfiguration Configuration { get; set; }
        Task<decimal> GetAccountBalanceAsync(string address, CancellationToken token = new CancellationToken());
        Web3 GetClient(IAccount account);
        Task<BigInteger> GetGasPriceAsync();
        Task<bool> ContractExist(string address, CancellationToken token = new());
        Task<IAccount> GetAdminAccountAsync(CancellationToken token = new CancellationToken());
        Task<IAccount> GetAccountAsync(string privateKey, BigInteger chainId= default, int index = 0, CancellationToken token = new CancellationToken());
        Task<string> DeployContractAsync<TDeployment>(IAccount account = null, Dictionary<string, object> parameters = null, CancellationToken token = new CancellationToken()) where TDeployment : ContractDeploymentMessage, new();
        Task<string> TransferFundsAsync(string destinationAddress, decimal ethereum, CancellationToken token = new CancellationToken());
        Task<string> TransferFundsAsync(IAccount source, string destinationAddress, decimal ethereum, CancellationToken token = new CancellationToken());
        Account CreateWallet();
        Wallet CreateHDWallet(string seed, string password);
        Account GetWalletAccount(string seed, string password, int index = 0);
        Task<List<EthTransaction>> GetContractTransactionsAsync(string accountAddress, string contractAddress,
            TransactionDirectionType directionType, string latestTransactionHex = null,
            CancellationToken token = new CancellationToken());

        Task<TransactionEstimate> GetDeploymentEstimateAsync<TDeployment>(IAccount account = null, Dictionary<string, object> parameters = null,
            CancellationToken token = new CancellationToken()) where TDeployment : ContractDeploymentMessage, new();

        Task<TransactionEstimate> GetTransferEstimateAsync(IAccount source, string contractAddress, string recipientAddress,
            CancellationToken token = new CancellationToken());
        
        Task<BigInteger> LatestBlockNumberAsync();
        Task<EthTransaction> GetTransferTransactionAsync(string hash, string accountAddress, string contractAddress,
            CancellationToken token = new CancellationToken());

        Task<EthTransaction> GetTransactionAsync(string hash,
            CancellationToken token = new CancellationToken());

        Task<TransactionEstimate> GetTransferEstimateAsync(IAccount source, string recipientAddress,
            CancellationToken token = new CancellationToken());

        Task<string> TransferFundsWithoutReceiptAsync(IAccount source, string destinationAddress, decimal ethereum,
            CancellationToken token = new CancellationToken());
    }
}