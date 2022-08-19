using System;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IEthereumContractFactory
    {
        Task<string> DeployERC20Async(Action<ERC20ContractConfiguration> buildConfig = null);
        Task<string> DeployCrowdsaleAsync(Action<CrowdsaleContractConfiguration> buildConfig = null);
        Task<string> DeployStakingAsync(Action<StakingContractConfiguration> buildConfig = null);
    }
}