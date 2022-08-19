using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Nethereum.RPC.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface ITokenService
    {
        Task<string> DeployERC20Async(string network, Action<ERC20ContractConfiguration> buildConfig = null);
        Task SetWalletAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null);
        Task<decimal> BalanceOfAsync(string network, string contractAddress, string accountAddress);
        Task<string> TransferAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount);
        Task<TransactionEstimate> EstimateERC20DeploymentAsync(string network, Action<ERC20ContractConfiguration> buildConfig = null);
        Task<TransactionEstimate> EstimateTransferAsync(string network, string contractAddress, string recipientAddress);
        Task<decimal> GetAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null);
        Task<string> BatchTransferAsync(string network, IAccount sender, string contractAddress, List<string> recipients, decimal amount);
        Task<string> TransferWithoutReceiptAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount);
        Task<string> TransferWithAuthorizationAsync(string network, IAccount sender, string ownerPrivateKey, string contractAddress, string recipient, decimal amount);
        Task<TokenDetails> GetDetailsAsync(string network, string contractAddress);
        Task<string> BatchTransferAsync(string network, IAccount sender, string contractAddress, List<string> recipients, List<decimal> amounts);

        Task<string> ApproveAsync(string network, IAccount sender, string contractAddress, string recipient,
            decimal amount);
    }
}