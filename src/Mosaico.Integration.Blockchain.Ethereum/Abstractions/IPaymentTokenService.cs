using System;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Nethereum.RPC.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IPaymentTokenService
    {
        Task SetWalletAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null);
        Task<decimal> BalanceOfAsync(string network, string contractAddress, string accountAddress);
        Task<string> TransferAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount);
        Task<TransactionEstimate> EstimateTransferAsync(string network, string contractAddress, string recipientAddress);
        Task<decimal> GetAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null);
    }
}