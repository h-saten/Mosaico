using System;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Models;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface ICrowdsaleService
    {
        Task<CrowdsaleContractConfiguration> DeployAsync(string network, Action<CrowdsaleContractConfiguration> build);
        Task StartNewStageAsync(string network, string contractAddress, Action<ContractStageConfiguration> buildConfig);
        Task PauseAsync(string network, string contractAddress, string privateKey);
        Task<string> BuyTokens(string network, string contractAddress, Action<BuyTokensConfiguration> buildConfig);
        Task<string> ExchangeTokens(string network, string contractAddress, Action<ExchangeTokensConfiguration> buildConfig);
        Task<TransactionEstimate> EstimateDeploymentAsync(string network, Action<CrowdsaleContractConfiguration> buildConfig = null);
        Task<decimal> WalletBalanceAsync(string network, string contractAddress, string walletAddress);
    }
}